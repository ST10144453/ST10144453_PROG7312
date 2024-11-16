using HtmlAgilityPack;
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Drawing; // Add the System.Drawing namespace
using System.Linq;
using System.Windows.Media;

public class WebScraperService
{
    public IEnumerable<EventModel> ScrapeEvents(string url)
    {
        var web = new HtmlWeb();
        var document = web.Load(url);

        var eventNodes = document.DocumentNode.SelectNodes("//div[@class='event-item']");

        var events = new List<EventModel>();
        foreach (var eventNode in eventNodes)
        {
            var eventModel = new EventModel
            {
                //eventTitle = eventNode.SelectSingleNode(".//h3").InnerText.Trim(),
                //eventDate = ParseEventDate(eventNode.SelectSingleNode(".//span[@class='event-date']").InnerText),
                //eventLocation = eventNode.SelectSingleNode(".//span[@class='event-location']").InnerText.Trim(),
                //eventDescription = eventNode.SelectSingleNode(".//div[@class='event-description']").InnerText.Trim(),
                //eventPhotos = GetEventPhotos(eventNode),
                //eventTags = GetEventTags(eventNode)
            };

            events.Add(eventModel);
        }

        return events;
    }

    private DateTime ParseEventDate(string dateString)
    {
        // Implement logic to parse the event date string
        // For example:
        return DateTime.Parse(dateString);
    }

    private List<string> GetEventPhotos(HtmlNode eventNode)
    {
        var photoNodes = eventNode.SelectNodes(".//img");
        return photoNodes.Select(n => n.GetAttributeValue("src", "")).ToList();
    }

    private List<TagsModel> GetEventTags(HtmlNode eventNode)
    {
        var tagNodes = eventNode.SelectNodes(".//a[@class='event-tag']");
        return tagNodes.Select(n => new TagsModel
        {
            TagId = int.Parse(n.GetAttributeValue("data-tag-id", "0")),
            TagName = n.InnerText.Trim(),
            TagBgColour = "#000000",
            TagFontColour = "ffffff"
        }).ToList();
    }
}
