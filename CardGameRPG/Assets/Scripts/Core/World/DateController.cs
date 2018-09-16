using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateController : MonoBehaviour {
    List<DaysOfWeek> daysOfWeek;
    List<WeeksOfMonth> weeksOfMonth;
    List<MonthsOfYear> monthsOfYear;

    public Date startDate;
    public int dayCount;

    public Text dayOfWeekText;
    public Text weekOfMonthText;
    public Text monthOfYearText;
    public Text yearText;

    private int daysInWeek;
    private int daysInMonth;
    private int daysInYear;

    private void Awake() {
        daysOfWeek = new List<DaysOfWeek>();
        foreach (DaysOfWeek day in Enum.GetValues(typeof(DaysOfWeek))) {
            daysOfWeek.Add(day);
        }
        weeksOfMonth = new List<WeeksOfMonth>();
        foreach (WeeksOfMonth week in Enum.GetValues(typeof(WeeksOfMonth))) {
            weeksOfMonth.Add(week);
        }
        monthsOfYear = new List<MonthsOfYear>();
        foreach (MonthsOfYear month in Enum.GetValues(typeof(MonthsOfYear))) {
            monthsOfYear.Add(month);
        }

        daysInWeek = daysOfWeek.Count;
        daysInMonth = daysInWeek * weeksOfMonth.Count;
        daysInYear = daysInMonth * monthsOfYear.Count;
        updateUI();
    }

    public void moveTimeForward(int daysForward) {
        dayCount += daysForward;
        updateUI();
    }

    public void updateUI() {
        Date curDate = getCurDate();
        dayOfWeekText.text = curDate.day.ToString();
        weekOfMonthText.text = curDate.week.ToString();
        monthOfYearText.text = curDate.month.ToString();
        yearText.text = curDate.year.ToString();
    }

    public Date getCurDate() {
        int remainingDays = dayCount;

        int yearsPassed = remainingDays / daysInYear;
        remainingDays -= (remainingDays / daysInYear) * daysInYear;

        int monthsPassed = remainingDays / daysInMonth;
        remainingDays -= (remainingDays / daysInMonth) * daysInMonth;

        int weeksPassed = remainingDays / daysInWeek;
        remainingDays -= (remainingDays / daysInMonth) * daysInWeek;

        int daysPassed = remainingDays;

        Date date = new Date();
        date.year = startDate.year + yearsPassed;
        date.month = monthsOfYear[(monthsOfYear.IndexOf(startDate.month) + monthsPassed) % monthsOfYear.Count];
        date.week = weeksOfMonth[(weeksOfMonth.IndexOf(startDate.week) + weeksPassed) % weeksOfMonth.Count];
        date.day = daysOfWeek[(daysOfWeek.IndexOf(startDate.day) + daysPassed) % daysOfWeek.Count];

        return date;
    }

    private static DateController dateController;
    public static DateController Instance() {
        if (!dateController) {
            dateController = FindObjectOfType<DateController>();
        }
        return dateController;
    }
}

[Serializable]
public struct Date {
    public DaysOfWeek day;
    public WeeksOfMonth week;
    public MonthsOfYear month;
    public int year;
}

