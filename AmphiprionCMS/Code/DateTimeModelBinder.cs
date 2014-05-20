using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//DateTime?
namespace AmphiprionCMS.Code
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            DateTime? dateTimeAttempt = GetA<DateTime>(bindingContext, "DateTime");
            if (dateTimeAttempt != null)
            {
                return dateTimeAttempt.Value;
            }
            Month = GetA<Int32>(bindingContext, "Month");
            Day = GetA<Int32>(bindingContext, "Day");
            Year = GetA<Int32>(bindingContext, "Year");
            Hour = GetA<Int32>(bindingContext, "Hour");
            Minute = GetA<Int32>(bindingContext, "Minute");

            DateTime? newDate = null;
            try
            {
                if (this.MonthDayYearSet && this.TimeSet)
                {
                    newDate = new DateTime(Year.Value, Month.Value, Day.Value, Hour.Value, Minute.Value, DateTime.MinValue.Second);
                }
                else if (MonthDayYearSet)
                {
                    newDate = new DateTime(Year.Value, Month.Value, Day.Value, DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
                }
                else if (TimeSet)
                {
                    newDate = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, Hour.Value, Minute.Value, DateTime.MinValue.Second);
                }
            }
            catch (Exception ex)
            {
                newDate = null;
            }

            //System.Web.Mvc.ModelMetadata modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => newDate, newDate.GetType());
            //ModelValidator compositeValidator = ModelValidator.GetModelValidator(modelMetadata, controllerContext);
            //foreach (ModelValidationResult result in compositeValidator.Validate(null))
            //{
            //    bindingContext.ModelState.AddModelError(result.MemberName, result.Message);
            //}


            return newDate;
        }
        private int? Month { get; set; }
        private int? Day { get; set; }
        private int? Year { get; set; }
        private int? Hour { get; set; }
        private int? Minute { get; set; }
        private Nullable<T> GetA<T>(ModelBindingContext bindingContext, string key) where T : struct
        {
            if (String.IsNullOrEmpty(key)) return null;
            ValueProviderResult valueResult;
            //Try it with the prefix...
            valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);
            //Didn't work? Try without the prefix if needed...
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix == true)
            {
                valueResult = bindingContext.ValueProvider.GetValue(key);
            }
            if (valueResult == null)
            {
                return null;
            }
            return (Nullable<T>)valueResult.ConvertTo(typeof(T));
        }

        public bool MonthDayYearSet { get { return Month.HasValue && Day.HasValue && Year.HasValue; } }

        public bool TimeSet { get { return Hour.HasValue && Minute.HasValue; } }

    }
}