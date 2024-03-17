﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Robotok.MVVM;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace Robotok.View.Grid
{
    public class SizeToStringObservableCollection : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //zoom count offset observablecollection
            var labelTexts = (SuppressNotifyObservableCollection<string>)values[3];

            if (!GridConverterFunctions.ValidateArray(values, 4))
                return labelTexts;

            double zoom = (double)values[0];
            int count = (int)values[1];
            int offset = (int)values[2];

            for (int i = 0; i < labelTexts.Count; i++)
                labelTexts[i] = string.Empty;

            labelTexts.SuppressNotify = true;
            while (GridConverterFunctions.NumberOfLabelsOnScreen(zoom) > labelTexts.Count)
                labelTexts.Add(string.Empty);
            labelTexts.SuppressNotify = false;


            var groupedAmountInOneBlock = GridConverterFunctions.AmountOfNumbersInOneLabel(zoom);

            int start = GridConverterFunctions.NumberOfLabelsToOmit(offset, zoom);
            int end = Math.Min(start + GridConverterFunctions.NumberOfLabelsOnScreen(zoom), count);


            if (groupedAmountInOneBlock == 1)
            {
                for (int i = start + 1; i <= end; i++)
                    labelTexts[i - 1 - start] = i.ToString();
                return labelTexts;
            }
            int numberOfBlocks = GridConverterFunctions.NumberOfLabels(count, zoom);
            end = Math.Min(start + GridConverterFunctions.NumberOfLabelsOnScreen(zoom), numberOfBlocks);

            for (int i = start; i < end; i++)
                labelTexts[i - start] = ($"{groupedAmountInOneBlock * i + 1}:");//-{groupedAmountInOneBlock * (i + 1)}
            return labelTexts;

        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StripSizeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0;

            double zoom = (double)value;

            return GridConverterFunctions.LabelLength(zoom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CoordinateToCanvasMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //x y 
            if (!GridConverterFunctions.ValidateArray(values, 2))
                return new Thickness(0, 0, 0, 0);
            int X = (int)values[0];
            int Y = (int)values[1];
            return new Thickness(GridConverterFunctions.unit * X, GridConverterFunctions.unit * Y, 0, 0);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitCanvasLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GridConverterFunctions.unit;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SizeToCanvasLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0;
            return (int)value * GridConverterFunctions.unit;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
