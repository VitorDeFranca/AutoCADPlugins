using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Internal;
using Autodesk.AutoCAD.Windows.Data;

namespace TitleBlocks.Model
{
    public class TBlock
    {
        //Etiquetas do selo
        public string _numRevisao { get; set; } = "Nº";
        public string _descRevisao { get; set; } = "DESCRIÇÃO";
        public string _prepRevisao { get; set; } = "PREP.";
        public string _aprovRevisao { get; set; } = "APROV.";
        public string _dataRevisao { get; set; } = "DATA";
        public string _elaboradoPor { get; set; } = "ELABORADO:";
        public string _verificadoPor { get; set; } = "VERIFICADO:";
        public string _aprovadoPor { get; set; } = "APROVADO:";
        public string _data { get; set; } = "DATA:";
        public string _gerenteProjeto { get; set; } = "GERENTE DE PROJETO";
        public string _giovani { get; set; } = "ENG. CIVIL: GIOVANI MOSER GIRARDI";
        public string _giovaniCrea { get; set; } = "CREA: 51.736/D-PR";
        public string _responsavelTecnico { get; set; } = "RESPONSÁVEL TÉCNICO";
        public string _botelho { get; set; } = "ENG. CIVIL: MARCELO MIRANDA BOTELHO";
        public string _botelhoCrea { get; set; } = "CREA: 73.456/D-MG";
        public string _nomeProjeto { get; set; } = "XXX";
        public string _titulo { get; set; } = "TÍTULO";
        public string _escala { get; set; } = "ESCALA";
        public string _docNum { get; set; } = "DOC. Nº XXXX-XX";
        public string _revisao1 { get; set; } = "REV.";
        public string _revisao2 { get; set; } = "REV.";
        public string _folha { get; set; } = "FOLHA:";
        public string _numCliente { get; set; } = "NÚMERO DO CLIENTE";
        public string _direitos { get; set; } = "DIREITOS AUTORAIS RESERVADOS - PROIBIDA QUALQUER REPRODUÇÃO SEM AUTORIZAÇÃO EXPRESSA";


        //Propriedades do projeto p/o selo
        public string empresa { get; set; } = "H  E  A  D  5     E  N  G  E  N  H  A  R  I  A";
        public string projeto { get; set; } = "XXXXX";
        public string primeiraLinha { get; set; } = "XXX";
        public string segundaLinha { get; set; } = "XXX";
        public string terceiraLinha { get; set; } = "XXX";
        public string quartaLinha { get; set; } = "XXX";
        public string numCliente { get; set; } = "FDZ-H5E-E-CFDE-XXX-XXXX";
        public string escala { get; set; } = "INDICADA";
        public string revisao1 { get; set; } = "0A";
        public string revisao2 { get; set; } = "00A";
        public string numDoc { get; set; } = "DE-XXX-XXX";
        public string folha { get; set; } = "X DE X";
        public string elaboradoPor { get; set; } = "CRO";
        public string verificadoPor { get; set; } = "LAGP";
        public string aprovadoPor { get; set; } = "MMB";
        public string data { get; set; } = "XXX/XX";


        public void DesenharSelo(double height, double width)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            //Para facilitar minha vida, pontos das bordas externas e internas:
            Point2d extInfEsq = new Point2d(0, 0);
            Point2d extSupEsq = new Point2d(0, height);
            Point2d extSupDir = new Point2d(width, height);
            Point2d extInfDir = new Point2d(width, 0);

            Point2d intInfEsq = new Point2d(width / 33.64, height / 59.4);
            Point2d intSupEsq = new Point2d(width / 33.64, height / 1.0171);
            Point2d intSupDir = new Point2d(width / 1.012, height / 1.0171);
            Point2d intInfDir = new Point2d(width / 1.012, height / 59.4);

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                //Abrindo a tabela de blocos
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                ObjectId blockRecId = ObjectId.Null;


                //Criando o bloco de selo
                BlockTableRecord btr = new BlockTableRecord();
                trans.GetObject(db.BlockTableId, OpenMode.ForWrite);

                Point3d seloInsertionPoint = new Point3d(0, 0, 0);
                btr.Origin = seloInsertionPoint;

                bt.UpgradeOpen();
                ObjectId btrId = bt.Add(btr);
                trans.AddNewlyCreatedDBObject(btr, true);

                //Borda externa do formato

                Polyline ple = new Polyline();
                ple.AddVertexAt(0, extInfEsq, 0, 0, 0);
                ple.AddVertexAt(1, extSupEsq, 0, 0, 0);
                ple.AddVertexAt(2, extSupDir, 0, 0, 0);
                ple.AddVertexAt(3, extInfDir, 0, 0, 0);
                ple.Closed = true;

                btr.AppendEntity(ple);
                trans.AddNewlyCreatedDBObject(ple, true);


                //Borda interna do formato
                Polyline pli = new Polyline();
                pli.AddVertexAt(0, intInfEsq, 0, 0, 0);
                pli.AddVertexAt(1, intSupEsq, 0, 0, 0);
                pli.AddVertexAt(2, intSupDir, 0, 0, 0);
                pli.AddVertexAt(3, intInfDir, 0, 0, 0);
                pli.Closed = true;
                pli.ColorIndex = 140;

                btr.AppendEntity(pli);
                trans.AddNewlyCreatedDBObject(pli, true);

                //Linhas da borda do formato
                //Line ln1 = new Line(new Point3d(0, 297, 0), new Point3d(25, 297, 0));

                //btr.AppendEntity(ln1);




                //Desenhando as linhas horizontais do selo
                Line ln1 = new Line();
                ln1.StartPoint = new Point3d(width / 1.2723, height / 51.652, 0);
                ln1.EndPoint = new Point3d(width / 1.01386, height / 51.652, 0);
                ln1.ColorIndex = 155;
                btr.AppendEntity(ln1);
                trans.AddNewlyCreatedDBObject(ln1, true);

                Line ln2 = new Line();
                ln2.StartPoint = new Point3d(width / 1.2723, height / 27.6279, 0);
                ln2.EndPoint = new Point3d(width / 1.01386, height / 27.6279, 0);
                ln2.ColorIndex = 155;
                btr.AppendEntity(ln2);
                trans.AddNewlyCreatedDBObject(ln2, true);

                Line ln3 = new Line();
                ln3.StartPoint = new Point3d(width / 1.2723, height / 18.8571, 0);
                ln3.EndPoint = new Point3d(width / 1.01386, height / 18.8571, 0);
                ln3.ColorIndex = 155;
                btr.AppendEntity(ln3);
                trans.AddNewlyCreatedDBObject(ln3, true);

                Line ln4 = new Line();
                ln4.StartPoint = new Point3d(width / 1.2723, height / 10.0678, 0);
                ln4.EndPoint = new Point3d(width / 1.01386, height / 10.0678, 0);
                ln4.ColorIndex = 6;
                btr.AppendEntity(ln4);
                trans.AddNewlyCreatedDBObject(ln4, true);

                Line ln5 = new Line();
                ln5.StartPoint = new Point3d(width / 1.2723, height / 8.6087, 0);
                ln5.EndPoint = new Point3d(width / 1.01386, height / 8.6087, 0);
                ln5.ColorIndex = 6;
                btr.AppendEntity(ln5);
                trans.AddNewlyCreatedDBObject(ln5, true);

                Line ln6 = new Line();
                ln6.StartPoint = new Point3d(width / 1.2723, height / 6.5275, 0);
                ln6.EndPoint = new Point3d(width / 1.01386, height / 6.5275, 0);
                ln6.ColorIndex = 6;
                btr.AppendEntity(ln6);
                trans.AddNewlyCreatedDBObject(ln6, true);

                Line ln7 = new Line();
                ln7.StartPoint = new Point3d(width / 1.2723, height / 6.1237, 0);
                ln7.EndPoint = new Point3d(width / 1.01386, height / 6.1237, 0);
                ln7.ColorIndex = 6;
                btr.AppendEntity(ln7);
                trans.AddNewlyCreatedDBObject(ln7, true);

                Line ln8 = new Line();
                ln8.StartPoint = new Point3d(width / 1.2723, height / 4.2580, 0);
                ln8.EndPoint = new Point3d(width / 1.01386, height / 4.2580, 0);
                ln8.ColorIndex = 6;
                btr.AppendEntity(ln8);
                trans.AddNewlyCreatedDBObject(ln8, true);

                Line ln9 = new Line();
                ln9.StartPoint = new Point3d(width / 1.2723, height / 3.9732, 0);
                ln9.EndPoint = new Point3d(width / 1.01386, height / 3.9732, 0);
                ln9.ColorIndex = 6;
                btr.AppendEntity(ln9);
                trans.AddNewlyCreatedDBObject(ln9, true);

                Line ln10 = new Line();
                ln10.StartPoint = new Point3d(width / 1.2723, height / 3.9208, 0);
                ln10.EndPoint = new Point3d(width / 1.01386, height / 3.9208, 0);
                ln10.ColorIndex = 155;
                btr.AppendEntity(ln10);
                trans.AddNewlyCreatedDBObject(ln10, true);

                Line ln11 = new Line();
                ln11.StartPoint = new Point3d(width / 1.2723, height / 3.7714, 0);
                ln11.EndPoint = new Point3d(width / 1.01386, height / 3.7714, 0);
                ln11.ColorIndex = 155;
                btr.AppendEntity(ln11);
                trans.AddNewlyCreatedDBObject(ln11, true);

                Line ln12 = new Line();
                ln12.StartPoint = new Point3d(width / 1.2723, height / 3.6330, 0);
                ln12.EndPoint = new Point3d(width / 1.01386, height / 3.6330, 0);
                ln12.ColorIndex = 155;
                btr.AppendEntity(ln12);
                trans.AddNewlyCreatedDBObject(ln12, true);

                Line ln13 = new Line();
                ln13.StartPoint = new Point3d(width / 1.2723, height / 3.5044, 0);
                ln13.EndPoint = new Point3d(width / 1.01386, height / 3.5044, 0);
                ln13.ColorIndex = 155;
                btr.AppendEntity(ln13);
                trans.AddNewlyCreatedDBObject(ln13, true);

                Line ln14 = new Line();
                ln14.StartPoint = new Point3d(width / 1.2723, height / 3.3846, 0);
                ln14.EndPoint = new Point3d(width / 1.01386, height / 3.3846, 0);
                ln14.ColorIndex = 155;
                btr.AppendEntity(ln14);
                trans.AddNewlyCreatedDBObject(ln14, true);

                //Desenhando as linhas verticais do selo
                Line ln15 = new Line();
                ln15.StartPoint = ln10.StartPoint;
                ln15.EndPoint = ln14.StartPoint;
                ln15.ColorIndex = 155;
                btr.AppendEntity(ln15);
                trans.AddNewlyCreatedDBObject(ln15, true);

                Line ln16 = new Line();
                ln16.StartPoint = new Point3d(width / 1.2533, height / 3.9208, 0);
                ln16.EndPoint = new Point3d(width / 1.2533, height / 3.3846, 0);
                ln16.ColorIndex = 155;
                btr.AppendEntity(ln16);
                trans.AddNewlyCreatedDBObject(ln16, true);

                Line ln17 = new Line();
                ln17.StartPoint = new Point3d(width / 1.0707, height / 3.9208, 0);
                ln17.EndPoint = new Point3d(width / 1.0707, height / 3.3846, 0);
                ln17.ColorIndex = 155;
                btr.AppendEntity(ln17);
                trans.AddNewlyCreatedDBObject(ln17, true);

                Line ln18 = new Line();
                ln18.StartPoint = new Point3d(width / 1.0532, height / 3.9208, 0);
                ln18.EndPoint = new Point3d(width / 1.0532, height / 3.3846, 0);
                ln18.ColorIndex = 155;
                btr.AppendEntity(ln18);
                trans.AddNewlyCreatedDBObject(ln18, true);

                Line ln19 = new Line();
                ln19.StartPoint = new Point3d(width / 1.0364, height / 3.9208, 0);
                ln19.EndPoint = new Point3d(width / 1.0364, height / 3.3846, 0);
                ln19.ColorIndex = 155;
                btr.AppendEntity(ln19);
                trans.AddNewlyCreatedDBObject(ln19, true);

                Line ln20 = new Line();
                ln20.StartPoint = ln10.EndPoint;
                ln20.EndPoint = ln14.EndPoint;
                ln20.ColorIndex = 155;
                btr.AppendEntity(ln20);
                trans.AddNewlyCreatedDBObject(ln20, true);

                Line ln21 = new Line();
                ln21.StartPoint = new Point3d(width / 1.1285, height / 4.3358, 0);
                ln21.EndPoint = new Point3d(width / 1.1285, height / 5.9698, 0);
                ln21.ColorIndex = 6;
                btr.AppendEntity(ln21);
                trans.AddNewlyCreatedDBObject(ln21, true);

                Line ln22 = new Line();
                ln22.StartPoint = new Point3d(width / 1.2009, height / 6.5275, 0);
                ln22.EndPoint = new Point3d(width / 1.2009, height / 6.1237, 0);
                ln22.ColorIndex = 6;
                btr.AppendEntity(ln22);
                trans.AddNewlyCreatedDBObject(ln22, true);

                Line ln23 = new Line();
                ln23.StartPoint = new Point3d(width / 1.1285, height / 6.1237, 0);
                ln23.EndPoint = new Point3d(width / 1.1285, height / 8.6087, 0);
                ln23.ColorIndex = 6;
                btr.AppendEntity(ln23);
                trans.AddNewlyCreatedDBObject(ln23, true);

                Line ln24 = new Line();
                ln24.StartPoint = new Point3d(width / 1.0642, height / 6.1237, 0);
                ln24.EndPoint = new Point3d(width / 1.0642, height / 6.5275, 0);
                ln24.ColorIndex = 6;
                btr.AppendEntity(ln24);
                trans.AddNewlyCreatedDBObject(ln24, true);

                Line ln25 = new Line();
                ln25.StartPoint = new Point3d(width / 1.2259, height / 51.652, 0);
                ln25.EndPoint = new Point3d(width / 1.2259, height / 18.8571, 0);
                ln25.ColorIndex = 155;
                btr.AppendEntity(ln25);
                trans.AddNewlyCreatedDBObject(ln25, true);

                Line ln26 = new Line();
                ln26.StartPoint = new Point3d(width / 1.046, height / 51.652, 0);
                ln26.EndPoint = new Point3d(width / 1.046, height / 18.8571, 0);
                ln26.ColorIndex = 155;
                btr.AppendEntity(ln26);
                trans.AddNewlyCreatedDBObject(ln26, true);

                //Linhas de assinatura
                Line ln27 = new Line();
                ln27.StartPoint = new Point3d(width / 1.2647, height / 7.425, 0);
                ln27.EndPoint = new Point3d(width / 1.1365, height / 7.425, 0);
                ln27.ColorIndex = 155;
                btr.AppendEntity(ln27);
                trans.AddNewlyCreatedDBObject(ln27, true);

                Line ln28 = new Line();
                ln28.StartPoint = new Point3d(width / 1.1225, height / 7.425, 0);
                ln28.EndPoint = new Point3d(width / 1.0203, height / 7.425, 0);
                ln28.ColorIndex = 155;
                btr.AppendEntity(ln28);
                trans.AddNewlyCreatedDBObject(ln28, true);


                //Inserindo as etiquetas no selo

                MText txLbNumRev = new MText();
                txLbNumRev.Contents = this._numRevisao;
                txLbNumRev.TextHeight = height / 297;
                double xCoord1 = width / 1.2648;
                double yCoord1 = height / 3.8172;
                Point3d insPt1 = new Point3d(xCoord1, yCoord1, 0);
                txLbNumRev.Location = insPt1;
                txLbNumRev.ColorIndex = 155;
                btr.AppendEntity(txLbNumRev);
                trans.AddNewlyCreatedDBObject(txLbNumRev, true);

                MText txLbDesc = new MText();
                txLbDesc.Contents = this._descRevisao;
                txLbDesc.TextHeight = height / 297;
                double xCoord2 = width / 1.1661;
                double yCoord2 = height / 3.8172;
                Point3d insPt2 = new Point3d(xCoord2, yCoord2, 0);
                txLbDesc.Location = insPt2;
                txLbDesc.ColorIndex = 155;
                btr.AppendEntity(txLbDesc);
                trans.AddNewlyCreatedDBObject(txLbDesc, true);

                MText txLbPrep = new MText();
                txLbPrep.Contents = this._prepRevisao;
                txLbPrep.TextHeight = height / 297;
                double xCoord3 = width / 1.0671;
                double yCoord3 = height / 3.8172;
                Point3d insPt3 = new Point3d(xCoord3, yCoord3, 0);
                txLbPrep.Location = insPt3;
                txLbPrep.ColorIndex = 155;
                btr.AppendEntity(txLbPrep);
                trans.AddNewlyCreatedDBObject(txLbPrep, true);

                MText txLbAprov = new MText();
                txLbAprov.Contents = this._aprovRevisao;
                txLbAprov.TextHeight = height / 297;
                double xCoord4 = width / 1.0512;
                double yCoord4 = height / 3.8172;
                Point3d insPt4 = new Point3d(xCoord4, yCoord4, 0);
                txLbAprov.Location = insPt4;
                txLbAprov.ColorIndex = 155;
                btr.AppendEntity(txLbAprov);
                trans.AddNewlyCreatedDBObject(txLbAprov, true);

                MText txLbDataRev = new MText();
                txLbDataRev.Contents = this._dataRevisao;
                txLbDataRev.TextHeight = height / 297;
                double xCoord5 = width / 1.0296;
                double yCoord5 = height / 3.8172;
                Point3d insPt5 = new Point3d(xCoord5, yCoord5, 0);
                txLbDataRev.Location = insPt5;
                txLbDataRev.ColorIndex = 155;
                btr.AppendEntity(txLbDataRev);
                trans.AddNewlyCreatedDBObject(txLbDataRev, true);

                MText txLbElab = new MText();
                txLbElab.Contents = this._elaboradoPor;
                txLbElab.TextHeight = height / 297;
                double xCoord6 = width / 1.2647;
                double yCoord6 = height / 6.2526;
                Point3d insPt6 = new Point3d(xCoord6, yCoord6, 0);
                txLbElab.Location = insPt6;
                txLbElab.ColorIndex = 155;
                btr.AppendEntity(txLbElab);
                trans.AddNewlyCreatedDBObject(txLbElab, true);

                MText txLbVer = new MText();
                txLbVer.Contents = this._verificadoPor;
                txLbVer.TextHeight = height / 297;
                double xCoord7 = width / 1.1942;
                double yCoord7 = height / 6.2526;
                Point3d insPt7 = new Point3d(xCoord7, yCoord7, 0);
                txLbVer.Location = insPt7;
                txLbVer.ColorIndex = 155;
                btr.AppendEntity(txLbVer);
                trans.AddNewlyCreatedDBObject(txLbVer, true);

                MText txLbAprovPor = new MText();
                txLbAprovPor.Contents = this._aprovadoPor;
                txLbAprovPor.TextHeight = height / 297;
                double xCoord8 = width / 1.1225;
                double yCoord8 = height / 6.2526;
                Point3d insPt8 = new Point3d(xCoord8, yCoord8, 0);
                txLbAprovPor.Location = insPt8;
                txLbAprovPor.ColorIndex = 155;
                btr.AppendEntity(txLbAprovPor);
                trans.AddNewlyCreatedDBObject(txLbAprovPor, true);

                MText txLbData = new MText();
                txLbData.Contents = this._data;
                txLbData.TextHeight = height / 297;
                double xCoord9 = width / 1.0589;
                double yCoord9 = height / 6.2526;
                Point3d insPt9 = new Point3d(xCoord9, yCoord9, 0);
                txLbData.Location = insPt9;
                txLbData.ColorIndex = 155;
                btr.AppendEntity(txLbData);
                trans.AddNewlyCreatedDBObject(txLbData, true);

                MText txLbGerenteProj = new MText();
                txLbGerenteProj.Contents = this._gerenteProjeto;
                txLbGerenteProj.TextHeight = height / 297;
                double xCoord10 = width / 1.2647;
                double yCoord10 = height / 6.7173;
                Point3d insPt10 = new Point3d(xCoord10, yCoord10, 0);
                txLbGerenteProj.Location = insPt10;
                txLbGerenteProj.ColorIndex = 155;
                btr.AppendEntity(txLbGerenteProj);
                trans.AddNewlyCreatedDBObject(txLbGerenteProj, true);

                MText txLbRespTec = new MText();
                txLbRespTec.Contents = this._responsavelTecnico;
                txLbRespTec.TextHeight = height / 297;
                double xCoord11 = width / 1.1225;
                double yCoord11 = height / 6.7173;
                Point3d insPt11 = new Point3d(xCoord11, yCoord11, 0);
                txLbRespTec.Location = insPt11;
                txLbRespTec.ColorIndex = 155;
                btr.AppendEntity(txLbRespTec);
                trans.AddNewlyCreatedDBObject(txLbRespTec, true);

                MText txLbGiovani = new MText();
                txLbGiovani.Contents = this._giovani;
                txLbGiovani.TextHeight = height / 297;
                double xCoord12 = width / 1.2647;
                double yCoord12 = height / 7.5974;
                Point3d insPt12 = new Point3d(xCoord12, yCoord12, 0);
                txLbGiovani.Location = insPt12;
                txLbGiovani.ColorIndex = 155;
                btr.AppendEntity(txLbGiovani);
                trans.AddNewlyCreatedDBObject(txLbGiovani, true);

                MText txLbBotelho = new MText();
                txLbBotelho.Contents = this._botelho;
                txLbBotelho.TextHeight = height / 297;
                double xCoord13 = width / 1.1225;
                double yCoord13 = height / 7.5974;
                Point3d insPt13 = new Point3d(xCoord13, yCoord13, 0);
                txLbBotelho.Location = insPt13;
                txLbBotelho.ColorIndex = 155;
                btr.AppendEntity(txLbBotelho);
                trans.AddNewlyCreatedDBObject(txLbBotelho, true);

                MText txLbCreaGiovani = new MText();
                txLbCreaGiovani.Contents = this._giovaniCrea;
                txLbCreaGiovani.TextHeight = height / 297;
                double xCoord14 = width / 1.2647;
                double yCoord14 = height / 8.1254;
                Point3d insPt14 = new Point3d(xCoord14, yCoord14, 0);
                txLbCreaGiovani.Location = insPt14;
                txLbCreaGiovani.ColorIndex = 155;
                btr.AppendEntity(txLbCreaGiovani);
                trans.AddNewlyCreatedDBObject(txLbCreaGiovani, true);

                MText txLbCreaBotelho = new MText();
                txLbCreaBotelho.Contents = this._botelhoCrea;
                txLbCreaBotelho.TextHeight = height / 297;
                double xCoord15 = width / 1.1225;
                double yCoord15 = height / 8.1254;
                Point3d insPt15 = new Point3d(xCoord15, yCoord15, 0);
                txLbCreaBotelho.Location = insPt15;
                txLbCreaBotelho.ColorIndex = 155;
                btr.AppendEntity(txLbCreaBotelho);
                trans.AddNewlyCreatedDBObject(txLbCreaBotelho, true);

                MText txLbTitulo = new MText();
                txLbTitulo.Contents = this._titulo;
                txLbTitulo.TextHeight = height / 297;
                double xCoord16 = width / 1.2704;
                double yCoord16 = height / 10.3352;
                Point3d insPt16 = new Point3d(xCoord16, yCoord16, 0);
                txLbTitulo.Location = insPt16;
                txLbTitulo.ColorIndex = 155;
                btr.AppendEntity(txLbTitulo);
                trans.AddNewlyCreatedDBObject(txLbTitulo, true);

                MText txLbEscala = new MText();
                txLbEscala.Contents = this._escala;
                txLbEscala.TextHeight = height / 297;
                double xCoord17 = width / 1.2704;
                double yCoord17 = height / 19.5205;
                Point3d insPt17 = new Point3d(xCoord17, yCoord17, 0);
                txLbEscala.Location = insPt17;
                txLbEscala.ColorIndex = 155;
                btr.AppendEntity(txLbEscala);
                trans.AddNewlyCreatedDBObject(txLbEscala, true);

                MText txLbFolha = new MText();
                txLbFolha.Contents = this._folha;
                txLbFolha.TextHeight = height / 297;
                double xCoord18 = width / 1.2704;
                double yCoord18 = height / 29.0203;
                Point3d insPt18 = new Point3d(xCoord18, yCoord18, 0);
                txLbFolha.Location = insPt18;
                txLbFolha.ColorIndex = 155;
                btr.AppendEntity(txLbFolha);
                trans.AddNewlyCreatedDBObject(txLbFolha, true);

                MText txLbDireitos = new MText();
                txLbDireitos.Contents = this._direitos;
                txLbDireitos.TextHeight = height / 297;
                double xCoord19 = width / 1.2704;
                double yCoord19 = height / 71.8094;
                Point3d insPt19 = new Point3d(xCoord19, yCoord19, 0);
                txLbDireitos.Location = insPt19;
                txLbDireitos.ColorIndex = 155;
                btr.AppendEntity(txLbDireitos);
                trans.AddNewlyCreatedDBObject(txLbDireitos, true);

                MText txLbDoc = new MText();
                txLbDoc.Contents = this._docNum;
                txLbDoc.TextHeight = height / 297;
                double xCoord20 = width / 1.2206;
                double yCoord20 = height / 19.5205;
                Point3d insPt20 = new Point3d(xCoord20, yCoord20, 0);
                txLbDoc.Location = insPt20;
                txLbDoc.ColorIndex = 155;
                btr.AppendEntity(txLbDoc);
                trans.AddNewlyCreatedDBObject(txLbDoc, true);

                MText txLbNumCliente = new MText();
                txLbNumCliente.Contents = this._numCliente;
                txLbNumCliente.TextHeight = height / 297;
                double xCoord21 = width / 1.2206;
                double yCoord21 = height / 29.0203;
                Point3d insPt21 = new Point3d(xCoord21, yCoord21, 0);
                txLbNumCliente.Location = insPt21;
                txLbNumCliente.ColorIndex = 155;
                btr.AppendEntity(txLbNumCliente);
                trans.AddNewlyCreatedDBObject(txLbNumCliente, true);

                MText txLbRevisao1 = new MText();
                txLbRevisao1.Contents = this._revisao1;
                txLbRevisao1.TextHeight = height / 297;
                double xCoord22 = width / 1.0441;
                double yCoord22 = height / 19.5205;
                Point3d insPt22 = new Point3d(xCoord22, yCoord22, 0);
                txLbRevisao1.Location = insPt22;
                txLbRevisao1.ColorIndex = 155;
                btr.AppendEntity(txLbRevisao1);
                trans.AddNewlyCreatedDBObject(txLbRevisao1, true);

                MText txLbRevisao2 = new MText();
                txLbRevisao2.Contents = this._revisao2;
                txLbRevisao2.TextHeight = height / 297;
                double xCoord23 = width / 1.0441;
                double yCoord23 = height / 29.0203;
                Point3d insPt23 = new Point3d(xCoord23, yCoord23, 0);
                txLbRevisao2.Location = insPt23;
                txLbRevisao2.ColorIndex = 155;
                btr.AppendEntity(txLbRevisao2);
                trans.AddNewlyCreatedDBObject(txLbRevisao2, true);


                //Inserindo as informações do projeto no selo

                MText txEmpresa = new MText();
                txEmpresa.Contents = this.empresa;
                txEmpresa.TextHeight = height / 148.5;
                double xCoord24 = width / 1.12847;
                double yCoord24 = height / 4.1230;
                Point3d insPt24 = new Point3d(xCoord24, yCoord24, 0);
                txEmpresa.Attachment = AttachmentPoint.MiddleCenter;
                txEmpresa.Location = insPt24;
                txEmpresa.ColorIndex = 7;
                btr.AppendEntity(txEmpresa);
                trans.AddNewlyCreatedDBObject(txEmpresa, true);

                MText txElab = new MText();
                txElab.Contents = this.elaboradoPor;
                txElab.TextHeight = height / 297;
                double xCoord25 = width / 1.2255;
                double yCoord25 = height / 6.2526;
                Point3d insPt25 = new Point3d(xCoord25, yCoord25, 0);
                txElab.Location = insPt25;
                txElab.ColorIndex = 155;
                btr.AppendEntity(txElab);
                trans.AddNewlyCreatedDBObject(txElab, true);

                MText txVer = new MText();
                txVer.Contents = this.verificadoPor;
                txVer.TextHeight = height / 297;
                double xCoord26 = width / 1.1592;
                double yCoord26 = height / 6.2526;
                Point3d insPt26 = new Point3d(xCoord26, yCoord26, 0);
                txVer.Location = insPt26;
                txVer.ColorIndex = 155;
                btr.AppendEntity(txVer);
                trans.AddNewlyCreatedDBObject(txVer, true);

                MText txAprov = new MText();
                txAprov.Contents = this.aprovadoPor;
                txAprov.TextHeight = height / 297;
                double xCoord27 = width / 1.0915;
                double yCoord27 = height / 6.2526;
                Point3d insPt27 = new Point3d(xCoord27, yCoord27, 0);
                txAprov.Location = insPt27;
                txAprov.ColorIndex = 155;
                btr.AppendEntity(txAprov);
                trans.AddNewlyCreatedDBObject(txAprov, true);

                MText txData = new MText();
                txData.Contents = this.data;
                txData.TextHeight = height / 297;
                double xCoord28 = width / 1.0444;
                double yCoord28 = height / 6.2526;
                Point3d insPt28 = new Point3d(xCoord28, yCoord28, 0);
                txData.Location = insPt28;
                txData.ColorIndex = 155;
                btr.AppendEntity(txData);
                trans.AddNewlyCreatedDBObject(txData, true);

                MText txProjeto = new MText();
                txProjeto.Contents = this.projeto;
                txProjeto.TextHeight = height / 148.5;
                double xCoord29 = width / 1.1285;
                double yCoord29 = height / 9.2239;
                Point3d insPt29 = new Point3d(xCoord29, yCoord29, 0);
                txProjeto.Attachment = AttachmentPoint.MiddleCenter;
                txProjeto.Location = insPt29;
                txProjeto.ColorIndex = 155;
                btr.AppendEntity(txProjeto);
                trans.AddNewlyCreatedDBObject(txProjeto, true);

                MText txPrimLinha = new MText();
                txPrimLinha.Contents = this.primeiraLinha;
                txPrimLinha.TextHeight = height / 148.5;
                double xCoord30 = width / 1.1285;
                double yCoord30 = height / 10.79096;
                Point3d insPt30 = new Point3d(xCoord30, yCoord30, 0);
                txPrimLinha.Attachment = AttachmentPoint.MiddleCenter;
                txPrimLinha.Location = insPt30;
                txPrimLinha.ColorIndex = 6;
                btr.AppendEntity(txPrimLinha);
                trans.AddNewlyCreatedDBObject(txPrimLinha, true);

                MText txSegLinha = new MText();
                txSegLinha.Contents = this.segundaLinha;
                txSegLinha.TextHeight = height / 148.5;
                double xCoord31 = width / 1.1285;
                double yCoord31 = height / 12.31685;
                Point3d insPt31 = new Point3d(xCoord31, yCoord31, 0);
                txSegLinha.Attachment = AttachmentPoint.MiddleCenter;
                txSegLinha.Location = insPt31;
                txSegLinha.ColorIndex = 7;
                btr.AppendEntity(txSegLinha);
                trans.AddNewlyCreatedDBObject(txSegLinha, true);

                MText txTerLinha = new MText();
                txTerLinha.Contents = this.terceiraLinha;
                txTerLinha.TextHeight = height / 148.5;
                double xCoord32 = width / 1.1285;
                double yCoord32 = height / 14.1768;
                Point3d insPt32 = new Point3d(xCoord32, yCoord32, 0);
                txTerLinha.Attachment = AttachmentPoint.MiddleCenter;
                txTerLinha.Location = insPt32;
                txTerLinha.ColorIndex = 7;
                btr.AppendEntity(txTerLinha);
                trans.AddNewlyCreatedDBObject(txTerLinha, true);

                MText txQuarLinha = new MText();
                txQuarLinha.Contents = this.quartaLinha;
                txQuarLinha.TextHeight = height / 148.5;
                double xCoord33 = width / 1.1285;
                double yCoord33 = height / 16.6472;
                Point3d insPt33 = new Point3d(xCoord33, yCoord33, 0);
                txQuarLinha.Attachment = AttachmentPoint.MiddleCenter;
                txQuarLinha.Location = insPt33;
                txQuarLinha.ColorIndex = 7;
                btr.AppendEntity(txQuarLinha);
                trans.AddNewlyCreatedDBObject(txQuarLinha, true);

                MText txEscala = new MText();
                txEscala.Contents = this.escala;
                txEscala.TextHeight = height / 237.6;
                double xCoord34 = width / 1.2628;
                double yCoord34 = height / 23.0304;
                Point3d insPt34 = new Point3d(xCoord34, yCoord34, 0);
                txEscala.Location = insPt34;
                txEscala.ColorIndex = 7;
                btr.AppendEntity(txEscala);
                trans.AddNewlyCreatedDBObject(txEscala, true);

                MText txNumDoc = new MText();
                txNumDoc.Contents = this.numDoc;
                txNumDoc.TextHeight = height / 148.5;
                double xCoord35 = width / 1.2206;
                double yCoord35 = height / 21.8124;
                Point3d insPt35 = new Point3d(xCoord35, yCoord35, 0);
                txNumDoc.Location = insPt35;
                txNumDoc.ColorIndex = 6;
                btr.AppendEntity(txNumDoc);
                trans.AddNewlyCreatedDBObject(txNumDoc, true);

                MText txRev1 = new MText();
                txRev1.Contents = this.revisao1;
                txRev1.TextHeight = height / 148.5;
                double xCoord36 = width / 1.0341;
                double yCoord36 = height / 21.8124;
                Point3d insPt36 = new Point3d(xCoord36, yCoord36, 0);
                txRev1.Location = insPt36;
                txRev1.ColorIndex = 6;
                btr.AppendEntity(txRev1);
                trans.AddNewlyCreatedDBObject(txRev1, true);

                MText txFolha = new MText();
                txFolha.Contents = this.folha;
                txFolha.TextHeight = height / 237.6;
                double xCoord37 = width / 1.2628;
                double yCoord37 = height / 38.4882;
                Point3d insPt37 = new Point3d(xCoord37, yCoord37, 0);
                txFolha.Location = insPt37;
                txFolha.ColorIndex = 7;
                btr.AppendEntity(txFolha);
                trans.AddNewlyCreatedDBObject(txFolha, true);

                MText txNumCliente = new MText();
                txNumCliente.Contents = this.numCliente;
                txNumCliente.TextHeight = height / 148.5;
                double xCoord38 = width / 1.2206;
                double yCoord38 = height / 35.3142;
                Point3d insPt38 = new Point3d(xCoord38, yCoord38, 0);
                txNumCliente.Location = insPt38;
                txNumCliente.ColorIndex = 6;
                btr.AppendEntity(txNumCliente);
                trans.AddNewlyCreatedDBObject(txNumCliente, true);

                MText txRev2 = new MText();
                txRev2.Contents = this.revisao2;
                txRev2.TextHeight = height / 148.5;
                double xCoord39 = width / 1.0341;
                double yCoord39 = height / 35.3142;
                Point3d insPt39 = new Point3d(xCoord39, yCoord39, 0);
                txRev2.Location = insPt39;
                txRev2.ColorIndex = 6;
                btr.AppendEntity(txRev2);
                trans.AddNewlyCreatedDBObject(txRev2, true);

                blockRecId = btr.Id;



                BlockTableRecord ms = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                BlockReference br = new BlockReference(Point3d.Origin, blockRecId);

                ms.AppendEntity(br);
                trans.AddNewlyCreatedDBObject(br, true);


                doc.SendStringToExecute("._zoom _all ", false, false, false);
                trans.Commit();
            }
        }

        /* public void DesenharSelo(double height, double width)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            //Para facilitar minha vida, pontos das bordas externas e internas:
            Point2d extInfEsq = new Point2d(0, 0);
            Point2d extSupEsq = new Point2d(0, height);
            Point2d extSupDir = new Point2d(width, height);
            Point2d extInfDir = new Point2d(width, 0);

            Point2d intInfEsq = new Point2d(width / 33.64, height / 59.4);
            Point2d intSupEsq = new Point2d(width / 33.64, height / 1.0171);
            Point2d intSupDir = new Point2d(width / 1.012, height / 1.0171);
            Point2d intInfDir = new Point2d(width / 1.012, height / 59.4);

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                //Abrindo a tabela de blocos
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Criando o bloco de selo
                BlockTableRecord btr = new BlockTableRecord();
                trans.GetObject(db.BlockTableId, OpenMode.ForWrite);

                btr.Name = "Selo";
                Point3d seloInsertionPoint = new Point3d(0, 0, 0);
                btr.Origin = seloInsertionPoint;

                bt.UpgradeOpen();
                ObjectId btrId = bt.Add(btr);
                trans.AddNewlyCreatedDBObject(btr, true);

                //Borda externa do formato

                Polyline ple = new Polyline();
                ple.AddVertexAt(0, extInfEsq, 0, 0, 0);
                ple.AddVertexAt(1, extSupEsq, 0, 0, 0);
                ple.AddVertexAt(2, extSupDir, 0, 0, 0);
                ple.AddVertexAt(3, extInfDir, 0, 0, 0);
                ple.Closed = true;

                btr.AppendEntity(ple);
                trans.AddNewlyCreatedDBObject(ple, true);


                //Borda interna do formato
                Polyline pli = new Polyline();
                pli.AddVertexAt(0, intInfEsq, 0, 0, 0);
                pli.AddVertexAt(1, intSupEsq, 0, 0, 0);
                pli.AddVertexAt(2, intSupDir, 0, 0, 0);
                pli.AddVertexAt(3, intInfDir, 0, 0, 0);
                pli.Closed = true;
                pli.ColorIndex = 140;

                btr.AppendEntity(pli);
                trans.AddNewlyCreatedDBObject(pli, true);

                //Linhas da borda do formato
                //Line ln1 = new Line(new Point3d(0, 297, 0), new Point3d(25, 297, 0));

                //btr.AppendEntity(ln1);




                //Desenhando as linhas horizontais do selo
                Line ln1 = new Line();
                ln1.StartPoint = new Point3d(width / 1.2723, height / 51.652, 0);
                ln1.EndPoint = new Point3d(width / 1.01386, height / 51.652, 0);
                ln1.ColorIndex = 155;
                btr.AppendEntity(ln1);
                trans.AddNewlyCreatedDBObject(ln1, true);

                Line ln2 = new Line();
                ln2.StartPoint = new Point3d(width / 1.2723, height / 27.6279, 0);
                ln2.EndPoint = new Point3d(width / 1.01386, height / 27.6279, 0);
                ln2.ColorIndex = 155;
                btr.AppendEntity(ln2);
                trans.AddNewlyCreatedDBObject(ln2, true);

                Line ln3 = new Line();
                ln3.StartPoint = new Point3d(width / 1.2723, height / 18.8571, 0);
                ln3.EndPoint = new Point3d(width / 1.01386, height / 18.8571, 0);
                ln3.ColorIndex = 155;
                btr.AppendEntity(ln3);
                trans.AddNewlyCreatedDBObject(ln3, true);

                Line ln4 = new Line();
                ln4.StartPoint = new Point3d(width / 1.2723, height / 10.0678, 0);
                ln4.EndPoint = new Point3d(width / 1.01386, height / 10.0678, 0);
                ln4.ColorIndex = 6;
                btr.AppendEntity(ln4);
                trans.AddNewlyCreatedDBObject(ln4, true);

                Line ln5 = new Line();
                ln5.StartPoint = new Point3d(width / 1.2723, height / 8.6087, 0);
                ln5.EndPoint = new Point3d(width / 1.01386, height / 8.6087, 0);
                ln5.ColorIndex = 6;
                btr.AppendEntity(ln5);
                trans.AddNewlyCreatedDBObject(ln5, true);

                Line ln6 = new Line();
                ln6.StartPoint = new Point3d(width / 1.2723, height / 6.5275, 0);
                ln6.EndPoint = new Point3d(width / 1.01386, height / 6.5275, 0);
                ln6.ColorIndex = 6;
                btr.AppendEntity(ln6);
                trans.AddNewlyCreatedDBObject(ln6, true);

                Line ln7 = new Line();
                ln7.StartPoint = new Point3d(width / 1.2723, height / 6.1237, 0);
                ln7.EndPoint = new Point3d(width / 1.01386, height / 6.1237, 0);
                ln7.ColorIndex = 6;
                btr.AppendEntity(ln7);
                trans.AddNewlyCreatedDBObject(ln7, true);

                Line ln8 = new Line();
                ln8.StartPoint = new Point3d(width / 1.2723, height / 4.2580, 0);
                ln8.EndPoint = new Point3d(width / 1.01386, height / 4.2580, 0);
                ln8.ColorIndex = 6;
                btr.AppendEntity(ln8);
                trans.AddNewlyCreatedDBObject(ln8, true);

                Line ln9 = new Line();
                ln9.StartPoint = new Point3d(width / 1.2723, height / 3.9732, 0);
                ln9.EndPoint = new Point3d(width / 1.01386, height / 3.9732, 0);
                ln9.ColorIndex = 6;
                btr.AppendEntity(ln9);
                trans.AddNewlyCreatedDBObject(ln9, true);

                Line ln10 = new Line();
                ln10.StartPoint = new Point3d(width / 1.2723, height / 3.9208, 0);
                ln10.EndPoint = new Point3d(width / 1.01386, height / 3.9208, 0);
                ln10.ColorIndex = 155;
                btr.AppendEntity(ln10);
                trans.AddNewlyCreatedDBObject(ln10, true);

                Line ln11 = new Line();
                ln11.StartPoint = new Point3d(width / 1.2723, height / 3.7714, 0);
                ln11.EndPoint = new Point3d(width / 1.01386, height / 3.7714, 0);
                ln11.ColorIndex = 155;
                btr.AppendEntity(ln11);
                trans.AddNewlyCreatedDBObject(ln11, true);

                Line ln12 = new Line();
                ln12.StartPoint = new Point3d(width / 1.2723, height / 3.6330, 0);
                ln12.EndPoint = new Point3d(width / 1.01386, height / 3.6330, 0);
                ln12.ColorIndex = 155;
                btr.AppendEntity(ln12);
                trans.AddNewlyCreatedDBObject(ln12, true);

                Line ln13 = new Line();
                ln13.StartPoint = new Point3d(width / 1.2723, height / 3.5044, 0);
                ln13.EndPoint = new Point3d(width / 1.01386, height / 3.5044, 0);
                ln13.ColorIndex = 155;
                btr.AppendEntity(ln13);
                trans.AddNewlyCreatedDBObject(ln13, true);

                Line ln14 = new Line();
                ln14.StartPoint = new Point3d(width / 1.2723, height / 3.3846, 0);
                ln14.EndPoint = new Point3d(width / 1.01386, height / 3.3846, 0);
                ln14.ColorIndex = 155;
                btr.AppendEntity(ln14);
                trans.AddNewlyCreatedDBObject(ln14, true);

                //Desenhando as linhas verticais do selo
                Line ln15 = new Line();
                ln15.StartPoint = ln10.StartPoint;
                ln15.EndPoint = ln14.StartPoint;
                ln15.ColorIndex = 155;
                btr.AppendEntity(ln15);
                trans.AddNewlyCreatedDBObject(ln15, true);

                Line ln16 = new Line();
                ln16.StartPoint = new Point3d(width / 1.2533, height / 3.9208, 0);
                ln16.EndPoint = new Point3d(width / 1.2533, height / 3.3846, 0);
                ln16.ColorIndex = 155;
                btr.AppendEntity(ln16);
                trans.AddNewlyCreatedDBObject(ln16, true);

                Line ln17 = new Line();
                ln17.StartPoint = new Point3d(width / 1.0707, height / 3.9208, 0);
                ln17.EndPoint = new Point3d(width / 1.0707, height / 3.3846, 0);
                ln17.ColorIndex = 155;
                btr.AppendEntity(ln17);
                trans.AddNewlyCreatedDBObject(ln17, true);

                Line ln18 = new Line();
                ln18.StartPoint = new Point3d(width / 1.0532, height / 3.9208, 0);
                ln18.EndPoint = new Point3d(width / 1.0532, height / 3.3846, 0);
                ln18.ColorIndex = 155;
                btr.AppendEntity(ln18);
                trans.AddNewlyCreatedDBObject(ln18, true);

                Line ln19 = new Line();
                ln19.StartPoint = new Point3d(width / 1.0364, height / 3.9208, 0);
                ln19.EndPoint = new Point3d(width / 1.0364, height / 3.3846, 0);
                ln19.ColorIndex = 155;
                btr.AppendEntity(ln19);
                trans.AddNewlyCreatedDBObject(ln19, true);

                Line ln20 = new Line();
                ln20.StartPoint = ln10.EndPoint;
                ln20.EndPoint = ln14.EndPoint;
                ln20.ColorIndex = 155;
                btr.AppendEntity(ln20);
                trans.AddNewlyCreatedDBObject(ln20, true);

                Line ln21 = new Line();
                ln21.StartPoint = new Point3d(width / 1.1285, height / 4.3358, 0);
                ln21.EndPoint = new Point3d(width / 1.1285, height / 5.9698, 0);
                ln21.ColorIndex = 6;
                btr.AppendEntity(ln21);
                trans.AddNewlyCreatedDBObject(ln21, true);

                Line ln22 = new Line();
                ln22.StartPoint = new Point3d(width / 1.2009, height / 6.5275, 0);
                ln22.EndPoint = new Point3d(width / 1.2009, height / 6.1237, 0);
                ln22.ColorIndex = 6;
                btr.AppendEntity(ln22);
                trans.AddNewlyCreatedDBObject(ln22, true);

                Line ln23 = new Line();
                ln23.StartPoint = new Point3d(width / 1.1285, height / 6.1237, 0);
                ln23.EndPoint = new Point3d(width / 1.1285, height / 8.6087, 0);
                ln23.ColorIndex = 6;
                btr.AppendEntity(ln23);
                trans.AddNewlyCreatedDBObject(ln23, true);

                Line ln24 = new Line();
                ln24.StartPoint = new Point3d(width / 1.0642, height / 6.1237, 0);
                ln24.EndPoint = new Point3d(width / 1.0642, height / 6.5275, 0);
                ln24.ColorIndex = 6;
                btr.AppendEntity(ln24);
                trans.AddNewlyCreatedDBObject(ln24, true);

                Line ln25 = new Line();
                ln25.StartPoint = new Point3d(width / 1.2259, height / 51.652, 0);
                ln25.EndPoint = new Point3d(width / 1.2259, height / 18.8571, 0);
                ln25.ColorIndex = 155;
                btr.AppendEntity(ln25);
                trans.AddNewlyCreatedDBObject(ln25, true);

                Line ln26 = new Line();
                ln26.StartPoint = new Point3d(width / 1.046, height / 51.652, 0);
                ln26.EndPoint = new Point3d(width / 1.046, height / 18.8571, 0);
                ln26.ColorIndex = 155;
                btr.AppendEntity(ln26);
                trans.AddNewlyCreatedDBObject(ln26, true);

                //Linhas de assinatura
                Line ln27 = new Line();
                ln27.StartPoint = new Point3d(width / 1.2647, height / 7.425, 0);
                ln27.EndPoint = new Point3d(width / 1.1365, height / 7.425, 0);
                ln27.ColorIndex = 155;
                btr.AppendEntity(ln27);
                trans.AddNewlyCreatedDBObject(ln27, true);

                Line ln28 = new Line();
                ln28.StartPoint = new Point3d(width / 1.1225, height / 7.425, 0);
                ln28.EndPoint = new Point3d(width / 1.0203, height / 7.425, 0);
                ln28.ColorIndex = 155;
                btr.AppendEntity(ln28);
                trans.AddNewlyCreatedDBObject(ln28, true);


                //Inserindo as etiquetas no selo

                MText txLbNumRev = new MText();
                txLbNumRev.Contents = this._numRevisao;
                txLbNumRev.TextHeight = height / 297;
                double xCoord1 = width / 1.2648;
                double yCoord1 = height / 3.8172;
                Point3d insPt1 = new Point3d(xCoord1, yCoord1, 0);
                txLbNumRev.Location = insPt1;
                txLbNumRev.ColorIndex = 155;
                btr.AppendEntity(txLbNumRev);
                trans.AddNewlyCreatedDBObject(txLbNumRev, true);

                MText txLbDesc = new MText();
                txLbDesc.Contents = this._descRevisao;
                txLbDesc.TextHeight = height / 297;
                double xCoord2 = width / 1.1661;
                double yCoord2 = height / 3.8172;
                Point3d insPt2 = new Point3d(xCoord2, yCoord2, 0);
                txLbDesc.Location = insPt2;
                txLbDesc.ColorIndex = 155;
                btr.AppendEntity(txLbDesc);
                trans.AddNewlyCreatedDBObject(txLbDesc, true);

                MText txLbPrep = new MText();
                txLbPrep.Contents = this._prepRevisao;
                txLbPrep.TextHeight = height / 297;
                double xCoord3 = width / 1.0671;
                double yCoord3 = height / 3.8172;
                Point3d insPt3 = new Point3d(xCoord3, yCoord3, 0);
                txLbPrep.Location = insPt3;
                txLbPrep.ColorIndex = 155;
                btr.AppendEntity(txLbPrep);
                trans.AddNewlyCreatedDBObject(txLbPrep, true);

                MText txLbAprov = new MText();
                txLbAprov.Contents = this._aprovRevisao;
                txLbAprov.TextHeight = height / 297;
                double xCoord4 = width / 1.0512;
                double yCoord4 = height / 3.8172;
                Point3d insPt4 = new Point3d(xCoord4, yCoord4, 0);
                txLbAprov.Location = insPt4;
                txLbAprov.ColorIndex = 155;
                btr.AppendEntity(txLbAprov);
                trans.AddNewlyCreatedDBObject(txLbAprov, true);

                MText txLbDataRev = new MText();
                txLbDataRev.Contents = this._dataRevisao;
                txLbDataRev.TextHeight = height / 297;
                double xCoord5 = width / 1.0296;
                double yCoord5 = height / 3.8172;
                Point3d insPt5 = new Point3d(xCoord5, yCoord5, 0);
                txLbDataRev.Location = insPt5;
                txLbDataRev.ColorIndex = 155;
                btr.AppendEntity(txLbDataRev);
                trans.AddNewlyCreatedDBObject(txLbDataRev, true);

                MText txLbElab = new MText();
                txLbElab.Contents = this._elaboradoPor;
                txLbElab.TextHeight = height / 297;
                double xCoord6 = width / 1.2647;
                double yCoord6 = height / 6.2526;
                Point3d insPt6 = new Point3d(xCoord6, yCoord6, 0);
                txLbElab.Location = insPt6;
                txLbElab.ColorIndex = 155;
                btr.AppendEntity(txLbElab);
                trans.AddNewlyCreatedDBObject(txLbElab, true);

                MText txLbVer = new MText();
                txLbVer.Contents = this._verificadoPor;
                txLbVer.TextHeight = height / 297;
                double xCoord7 = width / 1.1942;
                double yCoord7 = height / 6.2526;
                Point3d insPt7 = new Point3d(xCoord7, yCoord7, 0);
                txLbVer.Location = insPt7;
                txLbVer.ColorIndex = 155;
                btr.AppendEntity(txLbVer);
                trans.AddNewlyCreatedDBObject(txLbVer, true);

                MText txLbAprovPor = new MText();
                txLbAprovPor.Contents = this._aprovadoPor;
                txLbAprovPor.TextHeight = height / 297;
                double xCoord8 = width / 1.1225;
                double yCoord8 = height / 6.2526;
                Point3d insPt8 = new Point3d(xCoord8, yCoord8, 0);
                txLbAprovPor.Location = insPt8;
                txLbAprovPor.ColorIndex = 155;
                btr.AppendEntity(txLbAprovPor);
                trans.AddNewlyCreatedDBObject(txLbAprovPor, true);

                MText txLbData = new MText();
                txLbData.Contents = this._data;
                txLbData.TextHeight = height / 297;
                double xCoord9 = width / 1.0589;
                double yCoord9 = height / 6.2526;
                Point3d insPt9 = new Point3d(xCoord9, yCoord9, 0);
                txLbData.Location = insPt9;
                txLbData.ColorIndex = 155;
                btr.AppendEntity(txLbData);
                trans.AddNewlyCreatedDBObject(txLbData, true);

                MText txLbGerenteProj = new MText();
                txLbGerenteProj.Contents = this._gerenteProjeto;
                txLbGerenteProj.TextHeight = height / 297;
                double xCoord10 = width / 1.2647;
                double yCoord10 = height / 6.7173;
                Point3d insPt10 = new Point3d(xCoord10, yCoord10, 0);
                txLbGerenteProj.Location = insPt10;
                txLbGerenteProj.ColorIndex = 155;
                btr.AppendEntity(txLbGerenteProj);
                trans.AddNewlyCreatedDBObject(txLbGerenteProj, true);

                MText txLbRespTec = new MText();
                txLbRespTec.Contents = this._responsavelTecnico;
                txLbRespTec.TextHeight = height / 297;
                double xCoord11 = width / 1.1225;
                double yCoord11 = height / 6.7173;
                Point3d insPt11 = new Point3d(xCoord11, yCoord11, 0);
                txLbRespTec.Location = insPt11;
                txLbRespTec.ColorIndex = 155;
                btr.AppendEntity(txLbRespTec);
                trans.AddNewlyCreatedDBObject(txLbRespTec, true);

                MText txLbGiovani = new MText();
                txLbGiovani.Contents = this._giovani;
                txLbGiovani.TextHeight = height / 297;
                double xCoord12 = width / 1.2647;
                double yCoord12 = height / 7.5974;
                Point3d insPt12 = new Point3d(xCoord12, yCoord12, 0);
                txLbGiovani.Location = insPt12;
                txLbGiovani.ColorIndex = 155;
                btr.AppendEntity(txLbGiovani);
                trans.AddNewlyCreatedDBObject(txLbGiovani, true);

                MText txLbBotelho = new MText();
                txLbBotelho.Contents = this._botelho;
                txLbBotelho.TextHeight = height / 297;
                double xCoord13 = width / 1.1225;
                double yCoord13 = height / 7.5974;
                Point3d insPt13 = new Point3d(xCoord13, yCoord13, 0);
                txLbBotelho.Location = insPt13;
                txLbBotelho.ColorIndex = 155;
                btr.AppendEntity(txLbBotelho);
                trans.AddNewlyCreatedDBObject(txLbBotelho, true);

                MText txLbCreaGiovani = new MText();
                txLbCreaGiovani.Contents = this._giovaniCrea;
                txLbCreaGiovani.TextHeight = height / 297;
                double xCoord14 = width / 1.2647;
                double yCoord14 = height / 8.1254;
                Point3d insPt14 = new Point3d(xCoord14, yCoord14, 0);
                txLbCreaGiovani.Location = insPt14;
                txLbCreaGiovani.ColorIndex = 155;
                btr.AppendEntity(txLbCreaGiovani);
                trans.AddNewlyCreatedDBObject(txLbCreaGiovani, true);

                MText txLbCreaBotelho = new MText();
                txLbCreaBotelho.Contents = this._botelhoCrea;
                txLbCreaBotelho.TextHeight = height / 297;
                double xCoord15 = width / 1.1225;
                double yCoord15 = height / 8.1254;
                Point3d insPt15 = new Point3d(xCoord15, yCoord15, 0);
                txLbCreaBotelho.Location = insPt15;
                txLbCreaBotelho.ColorIndex = 155;
                btr.AppendEntity(txLbCreaBotelho);
                trans.AddNewlyCreatedDBObject(txLbCreaBotelho, true);

                MText txLbTitulo = new MText();
                txLbTitulo.Contents = this._titulo;
                txLbTitulo.TextHeight = height / 297;
                double xCoord16 = width / 1.2704;
                double yCoord16 = height / 10.3352;
                Point3d insPt16 = new Point3d(xCoord16, yCoord16, 0);
                txLbTitulo.Location = insPt16;
                txLbTitulo.ColorIndex = 155;
                btr.AppendEntity(txLbTitulo);
                trans.AddNewlyCreatedDBObject(txLbTitulo, true);

                MText txLbEscala = new MText();
                txLbEscala.Contents = this._escala;
                txLbEscala.TextHeight = height / 297;
                double xCoord17 = width / 1.2704;
                double yCoord17 = height / 19.5205;
                Point3d insPt17 = new Point3d(xCoord17, yCoord17, 0);
                txLbEscala.Location = insPt17;
                txLbEscala.ColorIndex = 155;
                btr.AppendEntity(txLbEscala);
                trans.AddNewlyCreatedDBObject(txLbEscala, true);

                MText txLbFolha = new MText();
                txLbFolha.Contents = this._folha;
                txLbFolha.TextHeight = height / 297;
                double xCoord18 = width / 1.2704;
                double yCoord18 = height / 29.0203;
                Point3d insPt18 = new Point3d(xCoord18, yCoord18, 0);
                txLbFolha.Location = insPt18;
                txLbFolha.ColorIndex = 155;
                btr.AppendEntity(txLbFolha);
                trans.AddNewlyCreatedDBObject(txLbFolha, true);

                MText txLbDireitos = new MText();
                txLbDireitos.Contents = this._direitos;
                txLbDireitos.TextHeight = height / 297;
                double xCoord19 = width / 1.2704;
                double yCoord19 = height / 71.8094;
                Point3d insPt19 = new Point3d(xCoord19, yCoord19, 0);
                txLbDireitos.Location = insPt19;
                txLbDireitos.ColorIndex = 155;
                btr.AppendEntity(txLbDireitos);
                trans.AddNewlyCreatedDBObject(txLbDireitos, true);

                MText txLbDoc = new MText();
                txLbDoc.Contents = this._docNum;
                txLbDoc.TextHeight = height / 297;
                double xCoord20 = width / 1.2206;
                double yCoord20 = height / 19.5205;
                Point3d insPt20 = new Point3d(xCoord20, yCoord20, 0);
                txLbDoc.Location = insPt20;
                txLbDoc.ColorIndex = 155;
                btr.AppendEntity(txLbDoc);
                trans.AddNewlyCreatedDBObject(txLbDoc, true);

                MText txLbNumCliente = new MText();
                txLbNumCliente.Contents = this._numCliente;
                txLbNumCliente.TextHeight = height / 297;
                double xCoord21 = width / 1.2206;
                double yCoord21 = height / 29.0203;
                Point3d insPt21 = new Point3d(xCoord21, yCoord21, 0);
                txLbNumCliente.Location = insPt21;
                txLbNumCliente.ColorIndex = 155;
                btr.AppendEntity(txLbNumCliente);
                trans.AddNewlyCreatedDBObject(txLbNumCliente, true);

                MText txLbRevisao1 = new MText();
                txLbRevisao1.Contents = this._revisao1;
                txLbRevisao1.TextHeight = height / 297;
                double xCoord22 = width / 1.0441;
                double yCoord22 = height / 19.5205;
                Point3d insPt22 = new Point3d(xCoord22, yCoord22, 0);
                txLbRevisao1.Location = insPt22;
                txLbRevisao1.ColorIndex = 155;
                btr.AppendEntity(txLbRevisao1);
                trans.AddNewlyCreatedDBObject(txLbRevisao1, true);

                MText txLbRevisao2 = new MText();
                txLbRevisao2.Contents = this._revisao2;
                txLbRevisao2.TextHeight = height / 297;
                double xCoord23 = width / 1.0441;
                double yCoord23 = height / 29.0203;
                Point3d insPt23 = new Point3d(xCoord23, yCoord23, 0);
                txLbRevisao2.Location = insPt23;
                txLbRevisao2.ColorIndex = 155;
                btr.AppendEntity(txLbRevisao2);
                trans.AddNewlyCreatedDBObject(txLbRevisao2, true);


                //Inserindo as informações do projeto no selo

                MText txEmpresa = new MText();
                txEmpresa.Contents = this.empresa;
                txEmpresa.TextHeight = height / 148.5;
                double xCoord24 = width / 1.12847;
                double yCoord24 = height / 4.1230;
                Point3d insPt24 = new Point3d(xCoord24, yCoord24, 0);
                txEmpresa.Attachment = AttachmentPoint.MiddleCenter;
                txEmpresa.Location = insPt24;
                txEmpresa.ColorIndex = 7;
                btr.AppendEntity(txEmpresa);
                trans.AddNewlyCreatedDBObject(txEmpresa, true);

                MText txElab = new MText();
                txElab.Contents = this.elaboradoPor;
                txElab.TextHeight = height / 297;
                double xCoord25 = width / 1.2255;
                double yCoord25 = height / 6.2526;
                Point3d insPt25 = new Point3d(xCoord25, yCoord25, 0);
                txElab.Location = insPt25;
                txElab.ColorIndex = 155;
                btr.AppendEntity(txElab);
                trans.AddNewlyCreatedDBObject(txElab, true);

                MText txVer = new MText();
                txVer.Contents = this.verificadoPor;
                txVer.TextHeight = height / 297;
                double xCoord26 = width / 1.1592;
                double yCoord26 = height / 6.2526;
                Point3d insPt26 = new Point3d(xCoord26, yCoord26, 0);
                txVer.Location = insPt26;
                txVer.ColorIndex = 155;
                btr.AppendEntity(txVer);
                trans.AddNewlyCreatedDBObject(txVer, true);

                MText txAprov = new MText();
                txAprov.Contents = this.aprovadoPor;
                txAprov.TextHeight = height / 297;
                double xCoord27 = width / 1.0915;
                double yCoord27 = height / 6.2526;
                Point3d insPt27 = new Point3d(xCoord27, yCoord27, 0);
                txAprov.Location = insPt27;
                txAprov.ColorIndex = 155;
                btr.AppendEntity(txAprov);
                trans.AddNewlyCreatedDBObject(txAprov, true);

                MText txData = new MText();
                txData.Contents = this.data;
                txData.TextHeight = height / 297;
                double xCoord28 = width / 1.0444;
                double yCoord28 = height / 6.2526;
                Point3d insPt28 = new Point3d(xCoord28, yCoord28, 0);
                txData.Location = insPt28;
                txData.ColorIndex = 155;
                btr.AppendEntity(txData);
                trans.AddNewlyCreatedDBObject(txData, true);

                MText txProjeto = new MText();
                txProjeto.Contents = this.projeto;
                txProjeto.TextHeight = height / 148.5;
                double xCoord29 = width / 1.1285;
                double yCoord29 = height / 9.2239;
                Point3d insPt29 = new Point3d(xCoord29, yCoord29, 0);
                txProjeto.Attachment = AttachmentPoint.MiddleCenter;
                txProjeto.Location = insPt29;
                txProjeto.ColorIndex = 155;
                btr.AppendEntity(txProjeto);
                trans.AddNewlyCreatedDBObject(txProjeto, true);

                MText txPrimLinha = new MText();
                txPrimLinha.Contents = this.primeiraLinha;
                txPrimLinha.TextHeight = height / 148.5;
                double xCoord30 = width / 1.1285;
                double yCoord30 = height / 10.79096;
                Point3d insPt30 = new Point3d(xCoord30, yCoord30, 0);
                txPrimLinha.Attachment = AttachmentPoint.MiddleCenter;
                txPrimLinha.Location = insPt30;
                txPrimLinha.ColorIndex = 6;
                btr.AppendEntity(txPrimLinha);
                trans.AddNewlyCreatedDBObject(txPrimLinha, true);

                MText txSegLinha = new MText();
                txSegLinha.Contents = this.segundaLinha;
                txSegLinha.TextHeight = height / 148.5;
                double xCoord31 = width / 1.1285;
                double yCoord31 = height / 12.31685;
                Point3d insPt31 = new Point3d(xCoord31, yCoord31, 0);
                txSegLinha.Attachment = AttachmentPoint.MiddleCenter;
                txSegLinha.Location = insPt31;
                txSegLinha.ColorIndex = 7;
                btr.AppendEntity(txSegLinha);
                trans.AddNewlyCreatedDBObject(txSegLinha, true);

                MText txTerLinha = new MText();
                txTerLinha.Contents = this.terceiraLinha;
                txTerLinha.TextHeight = height / 148.5;
                double xCoord32 = width / 1.1285;
                double yCoord32 = height / 14.1768;
                Point3d insPt32 = new Point3d(xCoord32, yCoord32, 0);
                txTerLinha.Attachment = AttachmentPoint.MiddleCenter;
                txTerLinha.Location = insPt32;
                txTerLinha.ColorIndex = 7;
                btr.AppendEntity(txTerLinha);
                trans.AddNewlyCreatedDBObject(txTerLinha, true);

                MText txQuarLinha = new MText();
                txQuarLinha.Contents = this.quartaLinha;
                txQuarLinha.TextHeight = height / 148.5;
                double xCoord33 = width / 1.1285;
                double yCoord33 = height / 16.6472;
                Point3d insPt33 = new Point3d(xCoord33, yCoord33, 0);
                txQuarLinha.Attachment = AttachmentPoint.MiddleCenter;
                txQuarLinha.Location = insPt33;
                txQuarLinha.ColorIndex = 7;
                btr.AppendEntity(txQuarLinha);
                trans.AddNewlyCreatedDBObject(txQuarLinha, true);

                MText txEscala = new MText();
                txEscala.Contents = this.escala;
                txEscala.TextHeight = height / 237.6;
                double xCoord34 = width / 1.2628;
                double yCoord34 = height / 23.0304;
                Point3d insPt34 = new Point3d(xCoord34, yCoord34, 0);
                txEscala.Location = insPt34;
                txEscala.ColorIndex = 7;
                btr.AppendEntity(txEscala);
                trans.AddNewlyCreatedDBObject(txEscala, true);

                MText txNumDoc = new MText();
                txNumDoc.Contents = this.numDoc;
                txNumDoc.TextHeight = height / 148.5;
                double xCoord35 = width / 1.2206;
                double yCoord35 = height / 21.8124;
                Point3d insPt35 = new Point3d(xCoord35, yCoord35, 0);
                txNumDoc.Location = insPt35;
                txNumDoc.ColorIndex = 6;
                btr.AppendEntity(txNumDoc);
                trans.AddNewlyCreatedDBObject(txNumDoc, true);

                MText txRev1 = new MText();
                txRev1.Contents = this.revisao1;
                txRev1.TextHeight = height / 148.5;
                double xCoord36 = width / 1.0341;
                double yCoord36 = height / 21.8124;
                Point3d insPt36 = new Point3d(xCoord36, yCoord36, 0);
                txRev1.Location = insPt36;
                txRev1.ColorIndex = 6;
                btr.AppendEntity(txRev1);
                trans.AddNewlyCreatedDBObject(txRev1, true);

                MText txFolha = new MText();
                txFolha.Contents = this.folha;
                txFolha.TextHeight = height / 237.6;
                double xCoord37 = width / 1.2628;
                double yCoord37 = height / 38.4882;
                Point3d insPt37 = new Point3d(xCoord37, yCoord37, 0);
                txFolha.Location = insPt37;
                txFolha.ColorIndex = 7;
                btr.AppendEntity(txFolha);
                trans.AddNewlyCreatedDBObject(txFolha, true);

                MText txNumCliente = new MText();
                txNumCliente.Contents = this.numCliente;
                txNumCliente.TextHeight = height / 148.5;
                double xCoord38 = width / 1.2206;
                double yCoord38 = height / 35.3142;
                Point3d insPt38 = new Point3d(xCoord38, yCoord38, 0);
                txNumCliente.Location = insPt38;
                txNumCliente.ColorIndex = 6;
                btr.AppendEntity(txNumCliente);
                trans.AddNewlyCreatedDBObject(txNumCliente, true);

                MText txRev2 = new MText();
                txRev2.Contents = this.revisao2;
                txRev2.TextHeight = height / 148.5;
                double xCoord39 = width / 1.0341;
                double yCoord39 = height / 35.3142;
                Point3d insPt39 = new Point3d(xCoord39, yCoord39, 0);
                txRev2.Location = insPt39;
                txRev2.ColorIndex = 6;
                btr.AppendEntity(txRev2);
                trans.AddNewlyCreatedDBObject(txRev2, true);

                BlockTableRecord ms = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                BlockReference br = new BlockReference(Point3d.Origin, btrId);

                ms.AppendEntity(br);
                trans.AddNewlyCreatedDBObject(br, true);


                //doc.SendStringToExecute("._zoom _all ", false, false, false);
                trans.Commit();
            }
        } */

    }
}
