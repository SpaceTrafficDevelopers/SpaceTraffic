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
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    public class LogoQuiz : Minigame
    {
        public static List<Logo> Logos { get; set; }

        private List<Question> questions;

        private const int NUMBER_OF_QUESTIONS = 30;
        private const int WIN_SCORE = 20;
        private Random random;

        public LogoQuiz() : base()
        {
            this.random = new Random();
        }

        public List<Question> getQuestions()
        {
            this.generateQuestions();
            return this.questions;
        }

        private void generateQuestions()
        {
            List<Logo> logos = new List<Logo>(Logos);
            this.questions = new List<Question>();

            for(int i = 0; i< NUMBER_OF_QUESTIONS; i++){
                Question question = createQuestion(i, logos);
                this.questions.Add(question);
            }
        }

        private Question createQuestion(int id, List<Logo> logos){
            int index = random.Next(logos.Count);
            
            Question question = new Question();
            question.Id = id;
            question.RightChoice = logos[index];
            
            logos.RemoveAt(index);
            createWrongChoices(question, logos);

            return question;
        }

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

        public bool checkAnswers(List<Answer> answers)
        {
            int score = 0;

            bool duplicateExists = answers.GroupBy(n => n.Id).Any(g => g.Count() > 1);
            bool lessThanMin = answers.Min(n => n.Id) < 0;
            bool biggerThanMax = answers.Max(n => n.Id) >= answers.Count;

            if (duplicateExists || lessThanMin || biggerThanMax)
                return false;

            foreach (Answer answer in answers)
            {
                Question question = this.questions[answer.Id];
                if (answer.SelectedAnswer.CompareTo(question.RightChoice) == 0)
                    score++;        
            }

            return score >= WIN_SCORE;
        }

    }

    public class Logo
    {
        public string Name { get; set; }

        public string ImageName { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }

        public Logo RightChoice { get; set; }

        public string FirstWrongChoice { get; set; }

        public string SecondWrongChoice { get; set; }
    }

    public class Answer{
        
        public int Id { get; set; }

        public string SelectedAnswer { get; set; } 
    }
}
