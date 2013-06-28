#region License

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0.html
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  
// Copyright (c) 2013, HTW Berlin

#endregion

using System;
using System.Collections.Generic;

namespace DateTimeGenerator
{
  public static class DateTimeGenerator
  {
    public static IEnumerable<DateTime> GenerateDailyRecurrence(this DateTime begin, int recurrenceInterval, int numberOfRecurrences)
    {
      yield return begin;

      var times = 1;

      while (true)
      {
        begin = begin.AddDays(recurrenceInterval);
        times++;
        if (times > numberOfRecurrences)
        {
          yield break;
        }
        yield return begin;
      }
    }

    public static IEnumerable<DateTime> GenerateDailyRecurrence(this DateTime begin, int recurrenceInterval, DateTime repeatUntilDate)
    {
      yield return begin;

      while (true)
      {
        begin = begin.AddDays(recurrenceInterval);
        if (begin.Date > repeatUntilDate.Date)
        {
          yield break;
        }
        yield return begin;
      }
    }

    public static IEnumerable<DateTime> GenerateMonthlyRecurrenceWithWeekDaySpecification(this DateTime begin, int recurrenceInterval, DateTime repeatUntilDate)
    {
      yield return begin;
      var startingDayOfWeek = begin.DayOfWeek;
      var weekDayIndexInMonth = GetweekDayIndexInMonth(begin);
      var recurringWeekDay = begin.DayOfWeek;

      while (true)
      {
        begin = begin.AddMonths(recurrenceInterval);
        var maxWeekDayIndexInMonth = GetMaximumWeekDayIndex(begin, startingDayOfWeek);
        int tempWeekDayIndexInMonth;
        if (maxWeekDayIndexInMonth < weekDayIndexInMonth)
        {
          tempWeekDayIndexInMonth = weekDayIndexInMonth - 1;
        }
        else
        {
          tempWeekDayIndexInMonth = weekDayIndexInMonth;
        }

        var weekDayIndexCounter = 0;
        var lastDayOfMonth = begin.GetLastDayOfMonth();
        for (var i = begin.GetFirstDayOfMonth(); i <= lastDayOfMonth; i = i.AddDays(1))
        {
          if (i.DayOfWeek == recurringWeekDay)
          {
            weekDayIndexCounter++;
            if (weekDayIndexCounter == tempWeekDayIndexInMonth)
            {
              i += begin.TimeOfDay;
              begin = i;
              break;
            }
          }
        }
        if (begin.Date > repeatUntilDate.Date)
        {
          yield break;
        }
        yield return begin;
      }
    }

    public static IEnumerable<DateTime> GenerateMonthlyRecurrenceWithWeekDaySpecification(this DateTime begin, int recurrenceInterval, int numberOfRecurrences)
    {
      yield return begin;
      var targetDayOfMonth = begin.DayOfWeek;
      var weekDayIndexInMonth = GetweekDayIndexInMonth(begin);
      var recurringWeekDay = begin.DayOfWeek;

      var times = 1;

      while (true)
      {
        begin = begin.AddMonths(recurrenceInterval);
        var maxWeekDayIndexInMonth = GetMaximumWeekDayIndex(begin, targetDayOfMonth);
        int tempWeekDayIndexInMonth;
        if (maxWeekDayIndexInMonth < weekDayIndexInMonth)
        {
          tempWeekDayIndexInMonth = weekDayIndexInMonth - 1;
        }
        else
        {
          tempWeekDayIndexInMonth = weekDayIndexInMonth;
        }

        var weekDayIndexCounter = 0;
        var lastDayOfMonth = begin.GetLastDayOfMonth();
        for (var i = begin.GetFirstDayOfMonth(); i <= lastDayOfMonth; i = i.AddDays(1))
        {
          if (i.DayOfWeek == recurringWeekDay)
          {
            weekDayIndexCounter++;
            if (weekDayIndexCounter == tempWeekDayIndexInMonth)
            {
              i += begin.TimeOfDay;
              begin = i;
              break;
            }
          }
        }
        times++;
        if (times > numberOfRecurrences)
        {
          yield break;
        }
        yield return begin;
      }
    }

    public static IEnumerable<DateTime> GenerateMonthlyRecurrenceWithCalendarDaySpecification(this DateTime begin, int recurrenceInterval, DateTime repeatUntilDate)
    {
      yield return begin;

      var targetDayOfMonth = begin.Day;

      while (true)
      {
        begin = begin.AddMonths(recurrenceInterval);
        var tempBegin = GetExistingBegin(begin, targetDayOfMonth);

        begin = tempBegin;
        if (begin.Date > repeatUntilDate.Date)
        {
          yield break;
        }
        yield return begin;
      }
    }

    public static IEnumerable<DateTime> GenerateMonthlyRecurrenceWithCalendarDaySpecification(this DateTime begin, int recurrenceInterval, int numberOfRecurrences)
    {
      yield return begin;

      var times = 1;
      var targetDayOfMonth = begin.Day;

      while (true)
      {
        begin = begin.AddMonths(recurrenceInterval);
        var tempBegin = GetExistingBegin(begin, targetDayOfMonth);

        begin = tempBegin;
        times++;
        if (times > numberOfRecurrences)
        {
          yield break;
        }
        yield return begin;
      }
    }

    public static IEnumerable<DateTime> GenerateWeeklyRecurrence(this DateTime begin, int recurrenceInterval, DayOfWeek[] weekDays, int numberOfRecurrences)
    {
      var times = 0;
      var weekDayRecurrenceCounter = 0;
      while (true)
      {
        if (weekDayRecurrenceCounter % recurrenceInterval == 0)
        {
          foreach (var dayOfWeek in weekDays)
          {
            if (dayOfWeek == begin.DayOfWeek)
            {
              times++;
              yield return begin;
            }
          }
        }

        begin = begin.AddDays(1);

        if (begin.DayOfWeek == DayOfWeek.Monday)
        {
          weekDayRecurrenceCounter++;
        }

        if (times >= numberOfRecurrences)
        {
          yield break;
        }
      }
    }

    public static IEnumerable<DateTime> GenerateWeeklyRecurrence(this DateTime begin, int recurrenceInterval, DayOfWeek[] weekDays, DateTime repeatUntilDate)
    {
      var weekDayRecurrenceCounter = 0;
      while (true)
      {
        if (weekDayRecurrenceCounter % recurrenceInterval == 0)
        {
          foreach (var dayOfWeek in weekDays)
          {
            if (dayOfWeek == begin.DayOfWeek)
            {
              yield return begin;
            }
          }
        }

        begin = begin.AddDays(1);

        if (begin.DayOfWeek == DayOfWeek.Monday)
        {
          weekDayRecurrenceCounter++;
        }

        if (begin.Date > repeatUntilDate.Date)
        {
          yield break;
        }
      }
    }

    public static IEnumerable<DateTime> GenerateYearlyRecurrence(this DateTime begin, int recurrenceInterval, int numberOfRecurrences)
    {
      yield return begin;

      var times = 1;

      while (true)
      {
        begin = begin.AddYears(recurrenceInterval);
        times++;
        if (times > numberOfRecurrences)
        {
          yield break;
        }
        yield return begin;
      }
    }

    public static IEnumerable<DateTime> GenerateYearlyRecurrence(this DateTime begin, int recurrenceInterval, DateTime repeatUntilDate)
    {
      yield return begin;

      while (true)
      {
        begin = begin.AddYears(recurrenceInterval);
        if (begin.Date > repeatUntilDate.Date)
        {
          yield break;
        }
        yield return begin;
      }
    }

    private static DateTime GetExistingBegin(DateTime begin, int targetDayOfMonth)
    {
      try
      {
        return new DateTime(begin.Year, begin.Month, targetDayOfMonth, begin.Hour, begin.Minute, begin.Second);
      }
      catch (ArgumentOutOfRangeException)
      {
        return GetExistingBegin(begin, targetDayOfMonth - 1);
      }
    }

    private static int GetMaximumWeekDayIndex(DateTime date, DayOfWeek weekDay)
    {
      var weekDayIndex = 0;
      for (var i = date.GetFirstDayOfMonth(); i <= date.GetLastDayOfMonth(); i = i.AddDays(1))
      {
        if (i.DayOfWeek == weekDay)
        {
          weekDayIndex++;
        }
      }
      return weekDayIndex;
    }

    private static int GetweekDayIndexInMonth(DateTime date)
    {
      var weekDayIndex = 0;
      for (var i = date.GetFirstDayOfMonth(); i <= date; i = i.AddDays(1))
      {
        if (i.DayOfWeek == date.DayOfWeek)
        {
          weekDayIndex++;
        }
      }
      return weekDayIndex;
    }
  }
}