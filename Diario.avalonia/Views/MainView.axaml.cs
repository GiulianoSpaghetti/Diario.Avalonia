using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Data.SQLite;

namespace Diario.avalonia.Views;

public partial class MainView : UserControl
{

    private static string cs = @"URI=file:" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\test.db";
    private static SQLiteConnection con;
    private static SQLiteCommand cmd;
    private static SQLiteDataReader rdr;
    private static string stm, s;
    public MainView()
    {
        InitializeComponent();
        con = new SQLiteConnection(cs);
        con.Open();
        cmd = new SQLiteCommand(con);

        cmd.CommandText = @"CREATE TABLE Diario(id INTEGER PRIMARY KEY, valore LONGTEXT UNIQUE, data DATE)";
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (SQLiteException e) {; }
        AggiornaEntita();
    }

    private void LeggiClicked(object sender, RoutedEventArgs e)
    {
        Errore.Content = "";
        try
        {
            stm = $"SELECT * FROM Diario WHERE id={GetIdFromEntita()}";
        }
        catch (Exception ex)
        {
            Errore.Content = ex.Message;
            return;
        }
        cmd = new SQLiteCommand(stm, con);
        rdr = cmd.ExecuteReader();
        try
        {
            rdr.Read();
        }
        catch (SQLiteException ex)
        {
            Errore.Content = ex.Message;
            return;
        }
        sstring.Text = $"{rdr.GetString(1)}";
        rdr.Close();
    }

    private void ModificaClicked(object sender, RoutedEventArgs e)
    {
        Errore.Content = "";
        try
        {
            stm = $"UPDATE Diario SET valore='{sstring.Text}', data=DATE('now') WHERE id={GetIdFromEntita()}";
        }
        catch (Exception ex)
        {
            Errore.Content = ex.Message;
            return;
        }

        cmd = new SQLiteCommand(stm, con);
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (SQLiteException ex)
        {
            Errore.Content = ex.Message;
            return;
        }

    }
    private void InserisciClicked(object sender, RoutedEventArgs e)
    {
        Errore.Content = "";
        cmd.CommandText = $"INSERT INTO Diario(valore, data) VALUES('{sstring.Text}', DATE('now'))";
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (SQLiteException ex)
        {
            Errore.Content = ex.Message;
            return;
        }
        AggiornaEntita();
    }
    private void EliminaClicked(object sender, RoutedEventArgs e)
    {
        Errore.Content = "";
        try
        {
            cmd.CommandText = $"DELETE FROM Diario WHERE id={GetIdFromEntita()}";
        }
        catch (Exception ex)
        {
            Errore.Content = ex.Message;
            return;
        }
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (SQLiteException ex)
        {
            Errore.Content = ex.Message;
            return;
        }
        AggiornaEntita();
    }
    private int GetIdFromEntita()
    {
        if (Dati.Items.Count == 0)
            throw new Exception("Database vuoto");
        s = Dati.SelectedItem.ToString();
        return Int32.Parse(s.Substring(0, s.IndexOf("-") - 1));

    }

    private void AggiornaEntita()
    {
        Dati.Items.Clear();
        stm = "SELECT * FROM Diario";

        cmd = new SQLiteCommand(stm, con);
        rdr = cmd.ExecuteReader();

        while (rdr.Read())
            Dati.Items.Add($"{rdr.GetInt32(0)} - {rdr.GetDateTime(2).ToString("dd-MM-yyyy")}");
        rdr.Close();
        Dati.SelectedIndex = 0;

    }

}
