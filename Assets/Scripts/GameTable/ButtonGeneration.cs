using System;
using System.Collections;
using System.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace GameTable
{
    public class ButtonGeneration : MonoBehaviour
    {
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private GameObject _parent;
        [SerializeField] private Button trueButton;
        [SerializeField] private Button falseButton;
        [SerializeField] private Button resultsButton;
        [SerializeField] private TextMeshProUGUI allAnswer;
        [SerializeField] private TextMeshProUGUI rightAnswersText;
        [SerializeField] private TextMeshProUGUI falseAnswersText;
        [SerializeField] private GameObject resultCanvas;
        [SerializeField] private TextMeshProUGUI levelResultText;
        [SerializeField] private GameObject workbookCanvas;
        [SerializeField] private Animator[] stars;
        private  int lvl;
        private  string levelKey = "Level";
        private bool anotherIsOn;
        private Button currentButton;
        private static Button[] buttons = new Button[5];
        private string[] _operators = {"+", "-", "*", "/"};
        int[] numbers = new int [buttons.Length * 2];
        int[] numbersComplex = new int [buttons.Length * 4];
        private static readonly int FillStar = Animator.StringToHash("FillStar");

        private void Start()
        {
            lvl = PlayerPrefs.GetInt(levelKey, 0);
            Cursor.visible = true;
            for (int i = 0; i < 5; i++)
            {
                buttons[i] = Instantiate(_buttonPrefab);
            }

            foreach (Button button in buttons)
            {
                button.transform.SetParent(_parent.transform);
                button.transform.localScale = new Vector3(1, 1, 1);
                button.gameObject.AddComponent<ChoosedItem>();
                button.GetComponent<ChoosedItem>().SetIsOn(false);
                currentButton = button;
                lvl = 3;
                switch (lvl)
                {
                    case 0:
                        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFormula().Replace(".",",");
                        break;
                    case 1:
                        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetComplexFormula().Replace(".",",");
                        break;
                    case 2: 
                        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFractionFormula().Replace(".",",");
                        break;
                    case 3: 
                        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetNewFormula().Replace(".",",");
                        break;
                }
                
                button.onClick.AddListener(() => ChangeImage(button.GetComponent<ChoosedItem>(), button));
            }

            resultsButton.onClick.AddListener(() => StartCoroutine(CheckResults()));
        }

        private IEnumerator CheckResults()
        {
          
            PlayerPrefs.SetInt(levelKey,lvl++);
            double rightAnswers = 0;
            double allAnswers = 0;
            foreach (Button button in buttons)
            {
                if (button.GetComponent<ChoosedItem>().CheckIfCorrect() &&
                    button.GetComponent<ChoosedItem>().GetIsSelected())
                {
                    rightAnswers++;
                }
                allAnswers++;
            }
            
            rightAnswersText.text = rightAnswers.ToString();
            falseAnswersText.text = (allAnswers - rightAnswers).ToString();
            var result = rightAnswers / allAnswers * 100;
            allAnswer.text = result + "%";
            // workbookCanvas.SetActive(false);
            trueButton.gameObject.SetActive(false);
            resultCanvas.SetActive(true);
            TimerScript.SetTimer(false);
            levelResultText.text = "";
            Debug.Log(result);
            Debug.Log("timescale " + Time.timeScale);
            if (result < 50.0)
            {
                levelResultText.text = "Can be better";
                
                yield return new WaitForSeconds(0.3f);
                stars[0].SetTrigger(FillStar);
            }
            else if (result < 90.0)
            {
                levelResultText.text = "Good";
                
                yield return new WaitForSeconds(0.3f);
                stars[0].SetTrigger(FillStar);
                
                yield return new WaitForSeconds(0.3f);
                stars[1].SetTrigger(FillStar);
            }
            else
            {
                levelResultText.text = "Excellent";
                
                yield return new WaitForSeconds(0.3f);
                stars[0].SetTrigger(FillStar);
                
                yield return new WaitForSeconds(0.3f);
                stars[1].SetTrigger(FillStar);
                
                yield return new WaitForSeconds(0.3f);
                stars[2].SetTrigger(FillStar);
            }
        }

        private string GetFormula()
        {
            var formula = "";
            for (var i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Random.Range(1, 12);
            }

            var firstNumber = (double) numbers[Random.Range(0, numbers.Length)];
            formula += (int) firstNumber + " ";

            var operators = _operators[Random.Range(0, _operators.Length)];
            formula += operators + " ";

            var secondNumber = (double) numbers[Random.Range(0, numbers.Length)];
            while (secondNumber > firstNumber)
            {
                secondNumber = numbers[Random.Range(0, numbers.Length)];
            }

            formula += (int) secondNumber + " = ";

            if (Random.Range(0, 2) == 1)
            {
                currentButton.GetComponent<ChoosedItem>().SetIsTrue(true);
                switch (operators)
                {
                    case "+":
                        formula += (firstNumber + secondNumber).ToString();
                        break;
                    case "-":
                        formula += (firstNumber - secondNumber).ToString();
                        break;
                    case "*":
                        formula += (firstNumber * secondNumber).ToString();
                        break;
                    case "/":
                        formula += Math.Round(firstNumber / secondNumber, 2).ToString();
                        break;
                }
            }
            else
            {
                currentButton.GetComponent<ChoosedItem>().SetIsTrue(false);
                switch (operators)
                {
                    case "+":
                        formula += (firstNumber + secondNumber + Random.Range(1, 4)).ToString();
                        break;
                    case "-":
                        formula += (firstNumber - secondNumber + Random.Range(1, 4)).ToString();
                        break;
                    case "*":
                        formula += (firstNumber * secondNumber + Random.Range(1, 4)).ToString();
                        break;
                    case "/":
                        formula += (Math.Round(firstNumber / secondNumber, 2) + +Random.Range(1, 4)).ToString();
                        break;
                }
            }
          
            return formula;
        }

        private string GetComplexFormula()
        {
            var formula = "";

            for (var i = 0; i < numbersComplex.Length; i++)
            {
                numbersComplex[i] = Random.Range(3, 20);
            }

            var firstNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) firstNumber + " ";
            var operatorFirst = _operators[Random.Range(0, _operators.Length)];
            formula += operatorFirst + " "; 
            var secondNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) secondNumber + " ";
            var operatorSecond = _operators[Random.Range(0, _operators.Length)];
            formula += operatorSecond + " ";
            var thirdNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) thirdNumber + " ";
            var operatorThird = _operators[Random.Range(0, _operators.Length)];
            formula += operatorThird + " ";
            var fourthNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) fourthNumber;
            ExpressionEvaluator.Evaluate(formula, out double result);
            result = Math.Round(result, 2);

            var checkForResult = result + Random.Range(-2, 2);
            formula += " = " + checkForResult;

            currentButton.GetComponent<ChoosedItem>().SetIsTrue(checkForResult == result);
          
            return formula;
        }
        
        private string GetFractionFormula()
        {
            var formula = "";

            for (var i = 0; i < numbersComplex.Length; i++)
            {
                numbersComplex[i] = Random.Range(1, 20);
            }

            var firstNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) firstNumber + "/" ;
            var secondNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) secondNumber + " ";
            var firstOperator = _operators[Random.Range(0, _operators.Length)];
            formula += firstOperator + " ";
            var thirdNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) thirdNumber + "/";
            var fourthNumber = (double) numbersComplex[Random.Range(0, numbersComplex.Length)];
            formula += (int) fourthNumber;
            ExpressionEvaluator.Evaluate(formula, out double result);
            result = Math.Round(result, 2);
            Fraction fraction = new Fraction();
            var checkForResult = result   + Random.Range(-2, 2);
            double temp = 0;
            if (checkForResult * 100 > 100 )
            {
                temp = Math.Round(checkForResult, 0);
                checkForResult -= temp;
                Debug.Log(temp);
                
            }
            
            fraction.numerator = (int)(checkForResult * 100);
            fraction.denominator = 100;
            fraction = ToReduce(fraction);
            if (temp >= 1)
            {
                formula += " = "+ temp + "+" + fraction.numerator + "/" + fraction.denominator;
            }
            else
            {
                formula += " = " + fraction.numerator + "/" + fraction.denominator;
            }
            
            currentButton.GetComponent<ChoosedItem>().SetIsTrue(checkForResult == result);
         
            return formula;
                
        }
        
        private String GetNewFormula()
        {
            var formula = "";
    
            for (var i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Random.Range(1, 20);
            }

            formula += "(";
            var firstNumber = numbers[Random.Range(0, numbers.Length)];
            formula += firstNumber + " ";
            var operatorFirst = _operators[Random.Range(0, 2)];
            formula += operatorFirst + " "; 
            var secondNumber = numbers[Random.Range(0, numbers.Length)];
            formula += secondNumber + ") ";
            var operatorSecond = _operators[Random.Range(2, _operators.Length)];
            formula += operatorSecond + " (";
            var thirdNumber = numbers[Random.Range(0, numbers.Length)];
            formula += thirdNumber + " ";
            var operatorThird = _operators[Random.Range(0, 2)];
            formula += operatorThird + " ";
            var fourthNumber = numbers[Random.Range(0, numbers.Length)];
            while (fourthNumber == thirdNumber)
            {
                fourthNumber = numbers[Random.Range(0, numbers.Length)];
            }
            formula += fourthNumber + ")";

            DataTable dataTable = new DataTable();
            var result = dataTable.Compute(formula, null);

            var checkForResult = Math.Round(Convert.ToDouble(result.ToString()), 2) + Random.Range(-2, 2);
            formula += " = " + checkForResult;
          
            return formula;
        }


       
        struct Fraction {
            public int numerator;
            public int denominator;
        }
        static Fraction ToReduce(Fraction fraction) {
            int nod = Nod(fraction.numerator, fraction.denominator);
            if (nod != 0) {
                fraction.numerator /= nod;
                fraction.denominator /= nod;
            }
            return fraction;
        }
        static int Nod(int n, int d) {
            int temp;
            n = Math.Abs(n);
            d = Math.Abs(d);
            while (d != 0 && n != 0) {
                if (n % d > 0) {
                    temp = n;
                    n = d;
                    d = temp % d;
                } else break;
            }
            if (d != 0 && n != 0) return d;
            else return 0;
        }
       

        void ChangeImage(ChoosedItem item, Button button)
        {
            CheckOn();
            if (!item.GetIsOn())
            {
                button.GetComponent<ChoosedItem>().SetIsSelected(false);
                item.SetIsOn(true);
                var text = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.fontStyle = FontStyles.Underline;
                trueButton.onClick.AddListener(() => trueAnswer(button));
                falseButton.onClick.AddListener(() => falseAnswer(button));
            }
            else
            {
                item.SetIsOn(false);
                var text = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.fontStyle = FontStyles.Normal;
            }
        }

        private void CheckOn()
        {
            anotherIsOn = false;
            foreach (var instrument in buttons)
            {
                if (instrument.GetComponent<ChoosedItem>().GetIsOn())
                {
                    var text = instrument.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    text.fontStyle = FontStyles.Normal;
                    trueButton.onClick.RemoveAllListeners();
                    falseButton.onClick.RemoveAllListeners();
                    anotherIsOn = true;
                }
            }
        }

        public void trueAnswer(Button button)
        {
            var text = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.color = Color.green;
            button.GetComponent<ChoosedItem>().SetSelectedTrue(true);
            button.GetComponent<ChoosedItem>().SetIsSelected(true);
            text.faceColor = new Color32(0, 160, 0, 255);
        }

        public void falseAnswer(Button button)
        {
            var text = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.color = Color.red;
            button.GetComponent<ChoosedItem>().SetSelectedTrue(false);
            button.GetComponent<ChoosedItem>().SetIsSelected(true);
            text.faceColor = Color.red;
        }
    }
}