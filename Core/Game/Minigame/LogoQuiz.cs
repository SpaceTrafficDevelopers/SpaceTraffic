using NLog;
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Logo quiz minigame.
    /// </summary>
    public class LogoQuiz : Minigame
    {
        /// <summary>
        /// List of all logos.
        /// </summary>
        public static List<Logo> Logos { get; set; }

        /// <summary>
        /// List of question.
        /// </summary>
        private List<Question> questions;

        /// <summary>
        /// Maximum nubmer of questions in one game.
        /// </summary>
        private const int NUMBER_OF_QUESTIONS = 30;

        /// <summary>
        /// Minimal score to win.
        /// </summary>
        private const int WIN_SCORE = 20;

        /// <summary>
        /// Random.
        /// </summary>
        private Random random;

        /// <summary>
        /// LogoQuiz constructor.
        /// </summary>
        public LogoQuiz() : base()
        {
            this.random = new Random();
        }

        /// <summary>
        /// Method for get list of questions for one game.
        /// </summary>
        /// <returns>return list of question</returns>
        public List<Question> getQuestions()
        {
            this.generateQuestions();
            return this.questions;
        }

        /// <summary>
        /// Method for generating list of unique question.
        /// </summary>
        private void generateQuestions()
        {
            List<Logo> logos = new List<Logo>(Logos);
            this.questions = new List<Question>();

            for(int i = 0; i< NUMBER_OF_QUESTIONS; i++){
                Question question = createQuestion(i, logos);
                this.questions.Add(question);
            }
        }

        /// <summary>
        /// Method for creating question with unique answers.
        /// </summary>
        /// <param name="id">question id</param>
        /// <param name="logos">list of all logos</param>
        /// <returns>question</returns>
        private Question createQuestion(int id, List<Logo> logos){
            int index = random.Next(logos.Count);
            
            Question question = new Question();
            question.Id = id;
            question.RightChoice = logos[index];
            
            logos.RemoveAt(index);
            createWrongChoices(question, logos);

            return question;
        }

        /// <summary>
        /// Method for creating wrong answers.
        /// </summary>
        /// <param name="question">question</param>
        /// <param name="logos">list of logo</param>
        private void createWrongChoices(Question question, List<Logo> logos)
        {
            int firstWrongChoiceIndex = random.Next(logos.Count);
            int secondWrongChoiceIndex = -1;
            
            do{
                secondWrongChoiceIndex = random.Next(logos.Count);
            }
            while(firstWrongChoiceIndex == secondWrongChoiceIndex);

            question.FirstWrongChoice = logos[firstWrongChoiceIndex].Name;
            question.SecondWrongChoice = logos[secondWrongChoiceIndex].Name;
        }

        /// <summary>
        /// Method for check list of answers.
        /// </summary>
        /// <param name="answersXml">answers in XML as string</param>
        /// <returns>true if player wins, otherwise false (even if player cheats)</returns>
        public bool checkAnswers(string answersXml)
        {         
            List<Answer> answers = this.parseAnswersXml(answersXml);

            int score = 0;

            if (answers == null || answers.Count == 0)
                return false;

            bool duplicateExists = answers.GroupBy(n => n.Id).Any(g => g.Count() > 1);
            bool lessThanMin = answers.Min(n => n.Id) < 0;
            bool biggerThanMax = answers.Max(n => n.Id) >= this.questions.Count;
            bool expectedNumberOfAnswers = answers.Count == NUMBER_OF_QUESTIONS; 

            if (duplicateExists || lessThanMin || biggerThanMax || !expectedNumberOfAnswers)
                return false;

            //check all questions and if list of answers does not contains any question from list
            //it is automatically false
            foreach (Question question in this.questions)
            {
                Answer answer = answers.FirstOrDefault(n => n.Id == question.Id);

                if (answer == null)
                    return false;

                if (answer.SelectedAnswer.CompareTo(question.RightChoice.Name) == 0)
                    score++;        
            }

            return score >= WIN_SCORE;
        }

        /// <summary>
        /// Method for parsing answers in xml.
        /// Format:
        /// <answers>
        ///     <answer>
        ///         <id>value</id>
        ///         <selectedAnswers>value</selectedAnswers>
        ///     </answer>
        /// </answers>
        /// </summary>
        /// <param name="answerXml">xml as string in format</param>
        /// <returns>list of answers</returns>
        private List<Answer> parseAnswersXml(string answerXml)
        {
            List<Answer> answers = new List<Answer>();

            try { 
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(answerXml);
                parseRootNode(doc.DocumentElement, answers);
                
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().InfoException("LogoQuiz: Error on parsing answers xml.", e);
            }

            return answers;
        }

        /// <summary>
        /// Method for parsing answer xml root node.
        /// </summary>
        /// <param name="root">root node</param>
        /// <param name="answers">empty list of answers</param>
        private void parseRootNode(XmlNode root, List<Answer> answers)
        {
            foreach (XmlNode answerNode in root.ChildNodes)
            {
                Answer answer = new Answer();

                foreach (XmlNode attr in answerNode.ChildNodes)
                {
                    switch (attr.Name)
                    {
                        case "id":
                            answer.Id = int.Parse(attr.InnerText);
                            break;
                        case "selectedAnswer":
                            answer.SelectedAnswer = attr.InnerText;
                            break;
                    }
                }

                answers.Add(answer);
            }
        }
    }

    /// <summary>
    /// Logo class.
    /// </summary>
    [DataContract]
    public class Logo
    {
        /// <summary>
        /// Logo name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Image name (with file extension).
        /// </summary>
        [DataMember]
        public string ImageName { get; set; }
    }

    /// <summary>
    /// Question class.
    /// </summary>
    [DataContract]
    public class Question
    {
        /// <summary>
        /// Question Id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Right choice.
        /// </summary>
        [DataMember]
        public Logo RightChoice { get; set; }

        /// <summary>
        /// First wrong choice.
        /// </summary>
        [DataMember]
        public string FirstWrongChoice { get; set; }

        /// <summary>
        /// Second wrong choice.
        /// </summary>
        [DataMember]
        public string SecondWrongChoice { get; set; }
    }

    /// <summary>
    /// Answer class.
    /// </summary>
    public class Answer{
        
        /// <summary>
        /// Answer Id. It should be same as question Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Selected answer.
        /// </summary>
        public string SelectedAnswer { get; set; } 
    }
}
