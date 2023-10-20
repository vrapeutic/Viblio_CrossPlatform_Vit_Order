using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel 
{
    void BeginLevel();
    void EndLevel();
    void SkipIntroLevel();
}
