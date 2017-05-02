using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using APIforEOSDIS;
#pragma warning disable 649

namespace ViewFinderChatBot.Logic
{
    public class SimpleViewFinderBot
    {
        //public 
        //public WorldviewQueryMaker querymaker { get; set; }
        
        public enum CountriesOptions { Ukraine, USA, Germany};
        //public enum CitiesOptions { Odessa, Washington, Berlin };

        public enum CategoriesOptions { Atmosphere, Cryosphere, Ocean};
        [Serializable]
        [Template(TemplateUsage.EnumSelectOne, "What kind of {&} would you like to see on the image? {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
        [Template(TemplateUsage.NotUnderstood, "I do not understand \"{0}\".", "Try again with differen word, please, I don't get \"{0}\".")]
        public class ViewFinderOrder
        {

            public CountriesOptions? Country;
            //[Optional]
            //public CitiesOptions? City;
            public CategoriesOptions Category;
            public static IForm<ViewFinderOrder> BuildForm()
            {
                OnCompletionAsyncDelegate<ViewFinderOrder> processOrder = async (context, state) =>
                {
                    RectArea rect=new RectArea();
                    string[] layers= { }; 
                    IMessageActivity reply = context.MakeMessage();
                    switch (state.Country)
                    {
                        case CountriesOptions.Ukraine: rect = new RectArea(18, 42, 53, 42); break;
                        case CountriesOptions.USA: rect = new RectArea(-146, 9, 50, -57); break;
                        case CountriesOptions.Germany: rect = new RectArea(1, 44, 55, 21); break;
                    }
                   
                    switch (state.Category)
                    {
                        case CategoriesOptions.Atmosphere : layers = new string[] { "AMSR2_Wind_Speed_Day" }; break;
                        case CategoriesOptions.Cryosphere : layers = new string[] { "MODIS_Terra_Sea_Ice", "MODIS_Aqua_Sea_Ice", "MODIS_Terra_Snow_Cover", "MODIS_Aqua_Snow_Cover" };break;
                        case CategoriesOptions.Ocean : layers = new string[] { "MODIS_Terra_Chlorophyll_A", "MODIS_Aqua_Chlorophyll_A" }; break;
                    }
                    WorldviewQueryMaker querymaker = new WorldviewQueryMaker(layers, new DateTime(2017, 04, 29), rect, projection_type.geographic);
                    reply.Text = string.Format(querymaker.Gen_Query_Link()); 
                    await context.PostAsync(reply);
                };

                return new FormBuilder<ViewFinderOrder>()
                        .Message("Hello! I am ViewFinder, the WorldView Searching help bot!")
                        .AddRemainingFields()
                        .Confirm("Your choice: {Country}; {Category}?")
                        .Message("Thanks you for using ViewFinder")
                        .OnCompletion(processOrder)
                        .Build();
                        
            }
        };
    }
}