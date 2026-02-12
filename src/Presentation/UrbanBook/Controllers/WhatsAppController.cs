using Application.Services.Interfaces;
using ExternalServices.Models.WhatsAppCloudModelRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController(IWhatsAppService WhatsAppService, IUtils util) : Controller
    {
        private readonly IWhatsAppService _WhatsAppService = WhatsAppService;
        private readonly IUtils _Util = util;

        [Authorize]
        [HttpGet("VerifyToken")]
        public IActionResult VerifyToken()
        {
            string AccessToken = "ASEDFRTGFRTR#$%^&%$RFDWEQAXCVGFFBNJK&*(^YHJNBGGGTRERDEefd";
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if (challenge != null && token != null && token == AccessToken)
            {
                return Ok(challenge);
            }
            else { return BadRequest(); }

        }

        [Authorize]
        [HttpGet("SendMessage")]
        public  IActionResult SendMessage()
        {
            
            //await _WhatsAppService.SendWhatsAppMessageAsync();

            return Ok("true");
        }

        [Authorize]
        [HttpPost]
        public IActionResult ReceiveMessage(WhatsAppCloudModel body)
        {
            try
            {
                //var Message = body?.Entry?.FirstOrDefault()?.Changes?.FirstOrDefault()?.Value?.Messages?.FirstOrDefault();
                //if (Message != null)
                //{
                //    var UserNumber = Message.From ?? string.Empty;
                //    var UserText = GetUserText(Message);
                //    object objectMessage;

                //    switch (UserText.ToUpper())
                //    {
                //        case "TEXT":
                //            objectMessage = _Util.TextMessage("Mensaje de prueba", UserNumber);
                //            break;
                //        case "IMAGE":
                //            objectMessage = _Util.ImageMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/image_whatsapp.png", UserNumber);
                //            break;
                //        case "AUDIO":
                //            objectMessage = _Util.AudioMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/audio_whatsapp.mp3", UserNumber);
                //            break;
                //        case "VIDEO":
                //            objectMessage = _Util.videoMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/video_whatsapp.mp4", UserNumber);
                //            break;
                //        case "DOCUMENT":
                //            objectMessage = _Util.DocumentMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/document_whatsapp.pdf", UserNumber);
                //            break;
                //        case "LOCATION":
                //            objectMessage = _Util.LocationMessage(UserNumber);
                //            break;
                //        case "BUTTON":
                //            objectMessage = _Util.ButtonsMessage(UserNumber);
                //            break;
                //        default:
                //            objectMessage = _Util.TextMessage("Lo siento no puedo entenderte", UserNumber);
                //            break;
                //    }

                //    await _WhatsAppService.Excute(objectMessage);
                //}
                return Ok("EVENT_RECEIVED");
            }
            catch (Exception)
            {

                return Ok("EVENT_RECEIVED");
            }

        }

        //private string GetUserText(Message message)
        //{
        //    string TypeMessage = message?.Type ?? string.Empty;

        //    if (TypeMessage.ToUpper().Equals("TEXT"))
        //    {
        //        return message?.Text?.Body ?? string.Empty;
        //    }
        //    else if (TypeMessage.ToUpper().Equals("INTERACTIVE"))
        //    {
        //        string interactiveType = message?.Interactive?.Type ?? string.Empty;

        //        if (interactiveType.ToUpper().Equals("LIST_REPLY"))
        //        {
        //            return message?.Interactive?.List_Reply?.Title ?? string.Empty;
        //        }
        //        else if (interactiveType.ToUpper().Equals("BUTTON_REPLY"))
        //        {
        //            return message?.Interactive?.Button_Reply?.Title ?? string.Empty;
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
    }
}
