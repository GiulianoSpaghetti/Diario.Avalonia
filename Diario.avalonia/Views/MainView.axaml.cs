﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Diario.Avalonia.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using SQLite;
using System.IO;

namespace Diario.avalonia.Views;

public partial class MainView : UserControl
{
    private static string cs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "test.db");
    private static SQLiteConnection con;
    private static string s;
    private static int id;
    private SQLite.TableQuery<Item> query;
    public static MainView Instance = null;
    public MainView()
    {
        InitializeComponent();
        con = new SQLiteConnection(cs);
        con.CreateTable<Item>();
        filtraPerData.SelectedDate=DateTime.Now;
        AggiornaEntita();
        Instance = this;
    }

    public static void Traduci ()
    {
        Instance.Leggi.Content = MainWindow.d["Leggi"] as string;
        Instance.Modifica.Content = MainWindow.d["Modifica"] as string;
        Instance.Elimina.Content = MainWindow.d["Elimina"] as string;
        Instance.Inserisci.Content = MainWindow.d["Inserisci"] as string;
        Instance.Cerca.Content = MainWindow.d["Ricerca"] as string;
    }

    private void LeggiClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            id = GetIdFromEntita();
        }
        catch (Exception ex)
        {
            MainWindow.MakeNotification(MainWindow.d["Errore"] as string, ex.Message);
            return;
        }
        query = con.Table<Item>().Where(v => v.Id.Equals(id));
        foreach (Item item in query)
        {
            sstring.Text = item.testo;
        }
    }

    private void ModificaClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            id = GetIdFromEntita();
        }
        catch (Exception ex)
        {
            MainWindow.MakeNotification(MainWindow.d["Errore"] as string, ex.Message);
            return;
        }
        query = con.Table<Item>().Where(v => v.Id.Equals(id));
        foreach (Item item in query)
        {
            item.testo = sstring.Text;
            item.data = DateTime.Now;
            con.Update(item);
        }
        AggiornaEntita();

    }
    private void InserisciClicked(object sender, RoutedEventArgs e)
    {
        Item item = new Item();
        item.data = DateTime.Now;
        item.testo = sstring.Text;
        con.Insert(item);
        AggiornaEntita();
    }
    private void EliminaClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            id = GetIdFromEntita();
        }
        catch (Exception ex)
        {
            MainWindow.MakeNotification(MainWindow.d["Errore"] as string, ex.Message);
            return;
        }
        query = con.Table<Item>().Where(v => v.Id.Equals(id));
        foreach (Item item in query)
        {
            con.Delete(item);
        }
        AggiornaEntita();
    }
    private void CercaDalClicked(object sender, RoutedEventArgs e) {
        AggiornaEntita(new DateTime(filtraPerData.SelectedDate.Value.Year, filtraPerData.SelectedDate.Value.Month, filtraPerData.SelectedDate.Value.Day));
    }

    private int GetIdFromEntita()
    {
        if (Dati.Items.Count == 0)
            throw new Exception(MainWindow.d["DatabaseVuoto"] as string);
        s = Dati.SelectedItem.ToString();
        return Int32.Parse(s.Substring(0, s.IndexOf("-") - 1));

    }

    private void AggiornaEntita(DateTime? data=null)
    {
        Dati.Items.Clear();
        List<Item> elementi;
        if (data == null)
            elementi = con.Table<Item>().ToList();
        else
        {
            query = con.Table<Item>().Where(v => v.data >= data);
            elementi = new List<Item>();
            foreach (Item item in query)
                elementi.Add(item);
        }
        if (elementi.Count > 0)
        {
            foreach (Item elemento in elementi)
                Dati.Items.Add($"{elemento.Id} - {elemento.data}");
            Dati.SelectedIndex = 0;
        }
        if (data != null)
            if (elementi.Count == 0)
                MainWindow.MakeNotification(MainWindow.d["ImpossibileTrovareElementi"] as string, MainWindow.d["ImpossibileTrovareElementi"] as string);
            else
                MainWindow.MakeNotification(MainWindow.d["RicercaEffettuata"] as string, $"{MainWindow.d["RicercaEffettuata"]} {MainWindow.d["CiSono"]} {elementi.Count} {MainWindow.d["elementi"]}.");
        Dati.IsEnabled = elementi.Count > 0;
        sstring.Text = "";
    }

}
