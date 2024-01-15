using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

[HarmonyPatch(typeof(HUDManager))]
class HUDManagerPatch
{
    [HarmonyPostfix, HarmonyPatch("OnEnable")]
    static void PostfixOnEnable()
    {
        HUDManager.Instance.chatTextField.onSubmit.AddListener(OnSubmitChat);
    }

    [HarmonyPostfix, HarmonyPatch("OnDisable")]
    static void PrefixOnDisable()
    {
        HUDManager.Instance.chatTextField.onSubmit.RemoveListener(OnSubmitChat);
    }

    static void OnSubmitChat(string chatString)
    {
        var localPlayer = GameNetworkManager.Instance.localPlayerController;
        if (!string.IsNullOrEmpty(chatString) && chatString.Length < 50)
        {
            HUDManager.Instance.AddTextToChatOnServer(chatString, (int)localPlayer.playerClientId);
        }
        for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Length; i++)
        {
            if (StartOfRound.Instance.allPlayerScripts[i].isPlayerControlled && Vector3.Distance(GameNetworkManager.Instance.localPlayerController.transform.position, StartOfRound.Instance.allPlayerScripts[i].transform.position) > 24.4f && (!GameNetworkManager.Instance.localPlayerController.holdingWalkieTalkie || !StartOfRound.Instance.allPlayerScripts[i].holdingWalkieTalkie))
            {
                HUDManager.Instance.playerCouldRecieveTextChatAnimator.SetTrigger("ping");
                break;
            }
        }
        localPlayer.isTypingChat = false;
        HUDManager.Instance.chatTextField.text = "";
        EventSystem.current.SetSelectedGameObject(null);
        HUDManager.Instance.PingHUDElement(HUDManager.Instance.Chat);
        HUDManager.Instance.typingIndicator.enabled = false;
    }

    [HarmonyPrefix, HarmonyPatch("SubmitChat_performed")]
    static void PrefixSubmitChat_performed(
        ref bool __runOriginal
    )
    {
        __runOriginal = false;
    }
}