using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private void OnMouseDown()
    {
        Web.Instance.SendCard(name);
    }
}
