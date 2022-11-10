using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Globalization;
using UnityEngine.UI;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

[Serializable]
public class MyJSON
{
    #region APP
    //public void FetchGames(string json)
    //{
    //    JSONNode n = JSON.Parse(json);
    //    GameManager.instance.STRA_gameID = new string[n.AsArray.Count];
    //    GameManager.instance.STRA_gameName = new string[n.AsArray.Count];
    //    GameManager.instance.STRA_templateName = new string[n.AsArray.Count];
    //    for (int i = 0; i < n.AsArray.Count; i++)
    //    {
    //        GameManager.instance.STRA_gameID[i] = n[i]["game_id"];
    //        GameManager.instance.STRA_gameName[i] = n[i]["game_name"];
    //        GameManager.instance.STRA_templateName[i] = n[i]["template_name"];
    //    }
    //}
    #endregion


    #region WEB
    public void FetchIDs()  // fetch child id and game id
    {
        JSONNode n = JSON.Parse(MainController.instance.STR_IDjson);
        MainController.instance.STR_childID = n["child_id"];
        MainController.instance.STR_GameID = n["game_id"];
    //    Debug.Log("CHILD ID FROM CRASH: " + MainController.instance.STR_childID);
     //   Debug.Log("GAME ID FROM CRASH: " + MainController.instance.STR_GameID);
    }

    public void THI_onGameComplete(string json)
    {
        JSONNode n = JSON.Parse(json);

        MainController.instance.STR_responseSerial = n["si_no"];
    }

    public void PassageClickTemp(string json)
    {
        JSONNode n = JSON.Parse(json);
        PassageClickManager.instance.STR_passage = n["options"][0]["option"];
        for(int i = 0; i <n["q_a"].AsArray.Count; i++)
        {
            PassageClickManager.instance.STRL_questions.Add(n["q_a"][i]["question"]);
            PassageClickManager.instance.STRL_answers.Add(n["q_a"][i]["answer"]); // answer
        }
    }

    public void Temp_type_1_LWS(string json, List<int> IL_numbers, List<string> STRL_instruction, List<string> STRL_instructionAudios, List<string> STRL_questions
                   , List<string> STRL_answers, List<string> STRL_quesitonAudios, List<string> STRL_questionID, List<string> STRL_options, List<string> STRL_optionAudios,List<string> STRL_details, List<string> STRL_cover_img_link,List<string> STRL_passageDetail)
    {
        Debug.Log(json);

        JSONNode n = JSON.Parse(json);

        IL_numbers.Add(n["q_a"].AsArray.Count); // total question count
        IL_numbers.Add(n["correct_points"]); // correct points
        IL_numbers.Add(n["wrong_points"]); // wrong points
        STRL_instruction.Add(n["instruction"]); // instruction
       // STRL_difficulty.Add(n["difficulty_level"]); // difficulty level
        STRL_instructionAudios.Add(n["instruction_audio"]);//instruction audio
        STRL_cover_img_link.Add(n["game_cover_img"]); // bg image link

        for (int i = 0; i < n["q_a"].AsArray.Count; i++)
        {
            STRL_questions.Add(n["q_a"][i]["question"]); // question
            STRL_answers.Add(n["q_a"][i]["answer"]); // answer
            STRL_quesitonAudios.Add(n["q_a"][i]["question_voice"]); // question audio
            STRL_questionID.Add(n["q_a"][i]["question_id"]); // question id
        }
        for (int i = 0; i < n["options"].AsArray.Count; i++)
        {
            STRL_options.Add(n["options"][i]["option"]); // option
            STRL_optionAudios.Add(n["options"][i]["option_voice"]); // option audioz
        }

        STRL_details.Add(n["child_name"]);
        STRL_details.Add(n["child_standard"]);
        STRL_details.Add(n["lesson_name"]);

        if (n["additional_keys"] != null)
        {
            Debug.Log(n["additional_keys"]);
            Debug.Log("Passage = " +n["additional_keys"]["passage"]);
            Debug.Log("Passage = " +n["additional_keys"]["passage"]["content_data"]);

            STRL_passageDetail.Add(n["additional_keys"]["passage"]["content_data"]);
            STRL_passageDetail.Add(n["additional_keys"]["passage"]["audio"]);
        }


    }


    public void Temp_type_1(string json, List<int> IL_numbers, List<string> STRL_difficulty, List<string> STRL_instruction, List<string> STRL_BG_img_link, List<string> STRL_instructionAudio, List<string> STRL_questions
                   , List<string> STRL_answers, List<string> STRL_quesitonAudios, List<string> STRL_questionID, List<string> STRL_options, List<string> STRL_optionAudios, List<string> STRL_avatar_Color, List<string> STRL_Panel_Img_link)
    {
        Debug.Log(json);
        JSONNode n = JSON.Parse(json);

        IL_numbers.Add(n["q_a"].AsArray.Count); // correct points
        IL_numbers.Add(n["correct_points"]);
        IL_numbers.Add(n["wrong_points"]); // wrong points
        STRL_difficulty.Add(n["difficulty_level"]); // difficulty mode
        STRL_instruction.Add(n["instruction"]); // game instruction
        STRL_BG_img_link.Add(n["bg_image"]); // bg image lin
      //  STRL_cover_img_link.Add(n["game_cover_img"]); // bg image link

        STRL_instructionAudio.Add(n["instruction_audio"]); // instruction audio

        for (int i = 0; i < n["q_a"].AsArray.Count; i++)
        {
            STRL_questions.Add(n["q_a"][i]["question"]); // question
            STRL_answers.Add(n["q_a"][i]["answer"]); // answer
            STRL_quesitonAudios.Add(n["q_a"][i]["question_voice"]); // question audio
            STRL_questionID.Add(n["q_a"][i]["question_id"]); // question id
        }
        for (int i = 0; i < n["options"].AsArray.Count; i++)
        {
            STRL_options.Add(n["options"][i]["option"]); // option
            STRL_optionAudios.Add(n["options"][i]["option_voice"]); // option audioz
        }
        if (n["customization"] != "")
        {
            STRL_avatar_Color.Add(n["customization"]["helicopter"]["customization_details"]["color"]);
            STRL_Panel_Img_link.Add(n["customization"]["option_holder"]["location"]);
        }

        
    }

    public void Temp_type_2(string json, List<string> STRL_difficulty, List<int> IL_numbers, List<string> STRL_questions, List<string> STRL_answers, List<string> STRL_options, 
        List<string> STRL_questionIDs, List<string> STRL_instruction, List<string> STRL_questionAudios, List<string> STRL_optionAudios, List<string> STRL_instructionAudios)
    {
        Debug.Log(json);

        JSONNode n = JSON.Parse(json);

        IL_numbers.Add(n["q_a"].AsArray.Count); // total question count
        IL_numbers.Add(n["correct_points"]); // correct points
        IL_numbers.Add(n["wrong_points"]); // wrong points
        STRL_instruction.Add(n["instruction"]); // instruction
        STRL_difficulty.Add(n["difficulty_level"]); // difficulty level
        STRL_instructionAudios.Add(n["instruction_audio"]);//instruction audio

        //STRL_cover_img_link.Add(n["game_cover_img"]); // bg image link

        for (int i = 0; i < n["q_a"].AsArray.Count; i++)
        {
            STRL_questions.Add(n["q_a"][i]["question"]); // question
            STRL_questionAudios.Add(n["q_a"][i]["question_voice"]); // question audio
            STRL_answers.Add(n["q_a"][i]["answer"]); // answer      
            STRL_questionIDs.Add(n["q_a"][i]["question_id"]); // question id

            for (int j = 0; j < n["q_a"][i]["options"].AsArray.Count; j++)
            {
                STRL_options.Add(n["q_a"][i]["options"][j]["option"]); // option
                STRL_optionAudios.Add(n["q_a"][i]["options"][j] ["option_voice"]); // option audio
            }
        }
    }


    public void Temp_type_2_LWS(string json, List<string> STRL_difficulty, List<int> IL_numbers, List<string> STRL_questions, List<string> STRL_answers, List<string> STRL_options, 
        List<string> STRL_questionIDs, List<string> STRL_instruction, List<string> STRL_questionAudios, List<string> STRL_optionAudios, 
        List<string> STRL_instructionAudios, List<string> STRL_details, List<string> STRL_cover_img_link)
    {
        Debug.Log(json);

        JSONNode n = JSON.Parse(json);

        IL_numbers.Add(n["q_a"].AsArray.Count); // total question count
        IL_numbers.Add(n["correct_points"]); // correct points
        IL_numbers.Add(n["wrong_points"]); // wrong points

        IL_numbers.Add(n["q_a"][0]["options"].AsArray.Count);

        STRL_instruction.Add(n["instruction"]); // instruction
        STRL_difficulty.Add(n["difficulty_level"]); // difficulty level
        STRL_instructionAudios.Add(n["instruction_audio"]);//instruction audio

        STRL_cover_img_link.Add(n["game_cover_img"]); // bg image link
        for (int i = 0; i < n["q_a"].AsArray.Count; i++)
        {
            STRL_questions.Add(n["q_a"][i]["question"]); // question
            STRL_questionAudios.Add(n["q_a"][i]["question_voice"]); // question audio
            STRL_answers.Add(n["q_a"][i]["answer"]); // answer      
            STRL_questionIDs.Add(n["q_a"][i]["question_id"]); // question id

            for (int j = 0; j < n["q_a"][i]["options"].AsArray.Count; j++)
            {
                STRL_options.Add(n["q_a"][i]["options"][j]["option"]); // option
                STRL_optionAudios.Add(n["q_a"][i]["options"][j]["option_voice"]); // option audio
            }
        }

        STRL_details.Add(n["child_name"]);
        STRL_details.Add(n["child_standard"]);
        STRL_details.Add(n["lesson_name"]);

    }
    #endregion
}





