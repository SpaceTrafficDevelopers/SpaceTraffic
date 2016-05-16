/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.Game.Minigame;
using System.Collections.Generic;
using SpaceTraffic.Entities;
using SpaceTraffic.Data.Minigame;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Text;
using SpaceTraffic.Utils.Tests;

namespace Core.Tests.Game.Minigame
{
    /// <summary>
    /// LogoQuiz test class.
    /// </summary>
    [TestClass]
    public class LogoQuizTest
    {
        /// <summary>
        /// Reference on tested instance.
        /// </summary>
        private LogoQuiz minigame;

        /// <summary>
        /// Minigame descriptor.
        /// </summary>
        private IMinigameDescriptor minigameDescriptor;

        /// <summary>
        /// Path to xml with logos.
        /// </summary>
        private const string xmlPath = @"Assets\Minigames\LogoQuiz\logos.xml";

        /// <summary>
        /// Initialization method. Creating minigame descriptor and spcaseship cargo finder instance.
        /// </summary>
        [TestInitialize()]
        public void TestInitialize()
        {
            string solutionPath = TestPath.getPathToSolution();
            string path = Path.Combine(solutionPath, xmlPath);

            LogoQuiz.Logos = LogoQuizLoader.loadLogos(path);

            this.minigameDescriptor = CreateMinigameDescriptor();

            this.minigame = new LogoQuiz();
            this.minigame.ID = 1;
            this.minigame.Descriptor = this.minigameDescriptor;
            this.minigame.CreateTime = DateTime.UtcNow;
            this.minigame.LastRequestTime = this.minigame.CreateTime;
            this.minigame.State = MinigameState.CREATED;
            this.minigame.Players = new Dictionary<int, Player>();
            this.minigame.FreeGame = true;
        }

        /// <summary>
        /// Test for creating expected logo quiz instance.
        /// This test contains tests for loading logos.
        /// </summary>
        [TestMethod()]
        public void ConsturctorTest()
        {
            Assert.IsNotNull(this.minigame);
            Assert.AreEqual(this.minigame.ID, 1);
            Assert.IsNotNull(this.minigame.Players);
            Assert.AreEqual(this.minigame.State, MinigameState.CREATED);
            Assert.AreEqual(this.minigame.CreateTime, this.minigame.LastRequestTime);
            Assert.IsNotNull(this.minigame.Descriptor);
            Assert.IsTrue(this.minigame.FreeGame);
            Assert.AreEqual(LogoQuiz.Logos.Count, 278);
        }


        /// <summary>
        /// Test for checking answers.
        /// </summary>
        [TestMethod()]
        public void CheckAnswersTest()
        {
            List<Question> questions = this.minigame.getQuestions();

            bool winResult = this.minigame.checkAnswers(generateAnswersXml(generateAnswers(questions, true)));
            bool looseResult = this.minigame.checkAnswers(generateAnswersXml(generateAnswers(questions, false)));
            
            Assert.IsTrue(winResult);
            Assert.IsFalse(looseResult);
        }

        /// <summary>
        /// Test for getting list of question.
        /// </summary>
        [TestMethod()]
        public void GetQuestions()
        {
            List<Question> questions = this.minigame.getQuestions();
            bool uniqueQuestion = !questions.GroupBy(n => n.RightChoice.Name).Any(g => g.Count() > 1);

            Assert.IsTrue(uniqueQuestion);

            for (int i = 0; i < questions.Count; i++)
            {
                Question quest = questions[i];
                bool uniqueFalseAnswers = quest.SecondWrongChoice.CompareTo(quest.FirstWrongChoice) != 0;
                bool differentFromRightAnswer = quest.FirstWrongChoice.CompareTo(quest.RightChoice.Name) != 0 &&
                    quest.SecondWrongChoice.CompareTo(quest.RightChoice.Name) != 0;

                Assert.IsTrue(uniqueQuestion, "False choices are same.");
                Assert.IsTrue(differentFromRightAnswer, "Right choice and one of the false choice (or both) are same.");
            }
        }

        /// <summary>
        /// Method for generating list of answers from questions.
        /// </summary>
        /// <param name="questions">questions</param>
        /// <param name="winning">true - generate winning answer</param>
        /// <returns>list of answers</returns>
        public List<Answer> generateAnswers(List<Question> questions, bool winning)
        {
            List<Answer> answers = new List<Answer>();

            for (int i = 0; i < questions.Count; i++)
            {
                Answer answer = new Answer();

                answer.Id = questions[i].Id;
                answer.SelectedAnswer = winning ? questions[i].RightChoice.Name : questions[i].FirstWrongChoice;
                
                answers.Add(answer);
            }

            return answers;
        }

        /// <summary>
        /// Method for generating xml from list of asnwers.
        /// </summary>
        /// <param name="answers">list of answers</param>
        /// <returns>list of answer in xml as string</returns>
        private string generateAnswersXml(List<Answer> answers){
            StringBuilder builder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(builder)) { 

                writer.WriteStartDocument();
                writer.WriteStartElement("answers");
            
                foreach(Answer answer in answers){
                    writer.WriteStartElement("answer");

                    writer.WriteElementString("id", answer.Id.ToString());
                    writer.WriteElementString("selectedAnswer", answer.SelectedAnswer);
                    
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Method for creating minigame descriptor for Logo Quiz.
        /// </summary>
        /// <returns></returns>
        private IMinigameDescriptor CreateMinigameDescriptor()
        {
            return new MinigameDescriptor
            {
                Name = "LogoQuiz",
                PlayerCount = 1,
                Description = "Hra, kde je hlavním úkolem uhádnout z neúplného loga o jakou instituci nebo společnost se jedná. " +
                              "Pokud uhádneš alspoň 20 z 30 log, dostaneš odměnu 1000 kreditů.",
                StartActions = new List<StartAction>(),
                RewardType = RewardType.CREDIT,
                SpecificReward = null,
                RewardAmount = 1000,
                ConditionType = ConditionType.CREDIT,
                ConditionArgs = "100",
                ExternalClient = true,
                MinigameClassFullName = "SpaceTraffic.Game.Minigame.LogoQuiz, SpaceTraffic.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                ClientURL = "path_to_apk_or_google_play"
            };
        }
    }
}
