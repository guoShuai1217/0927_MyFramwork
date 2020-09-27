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

public class JoinRoomDialog : UIDialogBase
{

    public Button btn_Back;

    private void Start()
    {
        btn_Back.onClick.AddListener(OnBackPressed);
    }

}