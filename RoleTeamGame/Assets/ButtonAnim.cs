using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class ButtonAnim : MonoBehaviour
    {

        public Animator animator;
        public bool setAnimButton;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ClickButton()
        {
            setAnimButton = true;

            animator.SetBool("IsPressed", setAnimButton);

            StartCoroutine(DisabledAnim());
        }

        IEnumerator DisabledAnim()
        {
            yield return new WaitForSeconds(.5f);

            setAnimButton = false;

            animator.SetBool("IsPressed", setAnimButton);
        }
    }
}

