using System;
using System.Collections.Generic;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class TagsModel
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string TagBgColour { get; set; }
        public string TagFontColour { get; set; }
        public bool IsSelected { get; set; } // Added IsSelected property


        public TagsModel()
        {
        }

        public static List<TagsModel> Tags => new List<TagsModel>
        {
           new TagsModel { TagId = 1, TagName = "Community Engagement", TagBgColour = "#4CAF50", TagFontColour = "#2E7D32" }, // Darker Green
new TagsModel { TagId = 2, TagName = "Cultural Festivals", TagBgColour = "#FF9800", TagFontColour = "#EF6C00" }, // Darker Orange
new TagsModel { TagId = 3, TagName = "Sports Events", TagBgColour = "#2196F3", TagFontColour = "#1565C0" }, // Darker Blue
new TagsModel { TagId = 4, TagName = "Health and Wellness", TagBgColour = "#9C27B0", TagFontColour = "#6A1B9A" }, // Darker Purple
new TagsModel { TagId = 5, TagName = "Environmental Initiatives", TagBgColour = "#8BC34A", TagFontColour = "#558B2F" }, // Darker Light Green
new TagsModel { TagId = 6, TagName = "Public Safety", TagBgColour = "#F44336", TagFontColour = "#C62828" }, // Darker Red
new TagsModel { TagId = 7, TagName = "Education and Workshops", TagBgColour = "#3F51B5", TagFontColour = "#1E88E5" }, // Darker Indigo
new TagsModel { TagId = 8, TagName = "Art and Craft Fairs", TagBgColour = "#FFEB3B", TagFontColour = "#F57F17" }, // Darker Yellow
new TagsModel { TagId = 9, TagName = "Local Markets", TagBgColour = "#E91E63", TagFontColour = "#C2185B" }, // Darker Pink
new TagsModel { TagId = 10, TagName = "Job Fairs", TagBgColour = "#00BCD4", TagFontColour = "#00838F" }, // Darker Cyan
new TagsModel { TagId = 11, TagName = "Historical Commemorations", TagBgColour = "#FF5722", TagFontColour = "#D84315" }, // Darker Deep Orange
new TagsModel { TagId = 12, TagName = "Family Activities", TagBgColour = "#FFC107", TagFontColour = "#FFA000" }, // Amber (No Change)
new TagsModel { TagId = 13, TagName = "Social Development", TagBgColour = "#607D8B", TagFontColour = "#37474F" }, // Darker Blue Grey
new TagsModel { TagId = 14, TagName = "Disaster Preparedness", TagBgColour = "#795548", TagFontColour = "#4E342E" }, // Darker Brown
new TagsModel { TagId = 15, TagName = "Public Consultations", TagBgColour = "#9E9E9E", TagFontColour = "#616161" }, // Darker Grey
new TagsModel { TagId = 16,  TagName = "Traffic and Transport", TagBgColour = "#00BFAE", TagFontColour = "#00796B" }, // Darker Teal
new TagsModel { TagId = 17, TagName = "Youth Programs", TagBgColour = "#CDDC39", TagFontColour = "#8BC34A" }, // Darker Lime
new TagsModel { TagId = 18, TagName = "Local Government Meetings", TagBgColour = "#D32F2F", TagFontColour = "#B71C1C" }, // Darker Red Darker
new TagsModel { TagId = 19, TagName = "Civic Participation", TagBgColour = "#D50000", TagFontColour = "#B71C1C" }, // Darker Red Accent
new TagsModel { TagId = 20, TagName = "Volunteer Opportunities", TagBgColour = "#8E24AA", TagFontColour = "#6A1B9A" } // Darker Purple

        };
    }
}
