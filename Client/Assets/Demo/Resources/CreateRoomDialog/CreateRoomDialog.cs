/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomDialog : UIDialogBase
{

    public override void OnClick(string str)
    {
        base.OnClick(str);
        switch (str)
        {
            case "btn_Back":
                OnBackPressed();
                break;

            case "btn_Sure":
                Debug.Log("Click btn_Sure");
                break;
            default:
                break;
        }
    }


}