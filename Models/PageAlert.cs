using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace UmbraChallenge.Models
{
    public enum AlertType {
            Success,
            Info,
            Warning,
            Danger
        }
    public class PageAlert(string message, AlertType type)
    {
        public MarkupString Message = new(message);
        public AlertType Type { get; set; } = type;
    }
}