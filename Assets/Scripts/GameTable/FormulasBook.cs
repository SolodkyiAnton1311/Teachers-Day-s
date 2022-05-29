using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameTable
{
    public class FormulasBook : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Button[] _firstStack;
        [SerializeField] private Button[] _secondStack;
        [SerializeField] private Button _goToNextFormulas;
        [SerializeField] private Button _goToPreviousFormulas;

        private void Start()
        {
            _goToPreviousFormulas.enabled = false;
            _goToNextFormulas.enabled = true;
            _goToNextFormulas.onClick.AddListener(() => StartCoroutine(ShowNextFormulas()));
            _goToPreviousFormulas.onClick.AddListener(() => StartCoroutine(ShowPreviousFormulas()));
        }

        private IEnumerator ShowNextFormulas()
        {
            _goToNextFormulas.enabled = false;
            _goToPreviousFormulas.enabled = true;
            foreach (var button in _firstStack)
            {
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }

            _animator.SetTrigger("Forward");
            yield return new WaitForSeconds(0.42f);
            // yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length+_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            foreach (var button in _secondStack)
            {
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }
        }

        private IEnumerator ShowPreviousFormulas()
        {
            _goToPreviousFormulas.enabled = false;
            _goToNextFormulas.enabled = true;
            foreach (var button in _secondStack)
            {
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }

            _animator.SetTrigger("Back");
            yield return new WaitForSeconds(0.42f);
            // yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length+_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            foreach (var button in _firstStack)
            {
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }
        }
    }
}