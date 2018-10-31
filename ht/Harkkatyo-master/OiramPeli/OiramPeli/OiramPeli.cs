using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class OiramPeli : PhysicsGame
{
    const double nopeus = 200;
    const double hyppyNopeus = 750;
    const int RUUDUN_KOKO = 40;

    PlatformCharacter pelaaja1;

    Image pelaajanKuva = LoadImage("HalloweenHahmo1.2");
    Image tahtiKuva = LoadImage("tahti");
    Image esteKuva = LoadImage("este1");
    Image taustaKuva = LoadImage("halloweenTausta2.png");


    SoundEffect maaliAani = LoadSoundEffect("maali");


    public override void Begin()
    {
        Gravity = new Vector(0, -1100);

        LuoKentta();
        LisaaNappaimet();

        Camera.Follow(pelaaja1);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
        /// MediaPlayer.Play("PianoPeli");
    }

    void LuoKentta()
    {
        /// TileMap kentta = TileMap.FromLevelAsset("kentta1");
        /// kentta.SetTileMethod('#', LisaaTaso);
        /// kentta.SetTileMethod('*', LisaaTahti);
        /// kentta.SetTileMethod('N', LisaaPelaaja);
        /// kentta.SetTileMethod('E', LisaaEste);
        /// kentta.Execute(RUUDUN_KOKO, RUUDUN_KOKO);
        /// Level.CreateBorders();
        /// Level.Background.CreateGradient(Color.White, Color.SkyBlue);
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("Kentta1");
        ruudut.SetTileMethod(Color.HanPurple, LisaaTaso);
        ruudut.SetTileMethod(Color.Black, LisaaPelaaja);
        ruudut.SetTileMethod(Color.Gold, LisaaTahti);
        ruudut.SetTileMethod(Color.Red, LisaaEste);
        /// Level.Background.Image = taustaKuva;
         
        ruudut.Execute(30, 30);
    }

    void LisaaTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Blue;
        Add(taso);
    }

    void LisaaTahti(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject tahti = PhysicsObject.CreateStaticObject(leveys, korkeus);
        tahti.IgnoresCollisionResponse = true;
        tahti.Position = paikka;
        tahti.Image = tahtiKuva;
        tahti.Tag = "tahti";
        Add(tahti);
    }

    void LisaaPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new PlatformCharacter(leveys, korkeus);
        pelaaja1.Position = paikka;
        pelaaja1.Mass = 4.0;
        pelaaja1.Image = pelaajanKuva;
        pelaaja1.Width = 28;
        pelaaja1.Height = 44;
        AddCollisionHandler(pelaaja1, "tahti", TormaaTahteen);
        AddCollisionHandler(pelaaja1, "Este", TormaaEsteeseen);
        Add(pelaaja1);
    }

    void LisaaEste(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Este = PhysicsObject.CreateStaticObject(leveys, korkeus);
        Este.Position = paikka;
        Este.Color = Color.Red;
        Este.Tag = "Este";
        Add(Este);
    }



    void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, -nopeus);
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, nopeus);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");

        ControllerOne.Listen(Button.DPadLeft, ButtonState.Down, Liikuta, "Pelaaja liikkuu vasemmalle", pelaaja1, -nopeus);
        ControllerOne.Listen(Button.DPadRight, ButtonState.Down, Liikuta, "Pelaaja liikkuu oikealle", pelaaja1, nopeus);
        ControllerOne.Listen(Button.A, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
    }

    void Liikuta(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Walk(nopeus);
    }

    void Hyppaa(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Jump(nopeus);
    }

    void TormaaTahteen(PhysicsObject hahmo, PhysicsObject tahti)
    {
        maaliAani.Play();
        MessageDisplay.Add("Keräsit tähden!");
        tahti.Destroy();
    }

    void TormaaEsteeseen(PhysicsObject hahmo, PhysicsObject Este)
    {
        /// To do: "play again" nappula
        maaliAani.Play();
        MessageDisplay.Add("Kuolit!!!");
        pelaaja1.Destroy();
    }

}
