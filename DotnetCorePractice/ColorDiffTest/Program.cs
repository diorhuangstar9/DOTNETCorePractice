using System;
using Colourful;

namespace ColorDiffTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IColorConverter<RGBColor, LabColor> converter = new ConverterBuilder()
                .FromRGB(RGBWorkingSpaces.sRGB)
                .ToLab(Illuminants.D65)
                .Build();

            var argbColor = System.Drawing.Color.FromArgb(
                0, 254, 255);
            var rgbColor1 = new RGBColor(System.Drawing.Color.FromArgb(100,
                0, 100, 255));
            var rgbColor2 = new RGBColor(System.Drawing.Color.FromArgb(10,
                0, 100, 255));
            Console.WriteLine(rgbColor1.ToString());
            Console.WriteLine(rgbColor2.ToString());

            var labColor1 = converter.Convert(rgbColor1);
            var labColor2 = converter.Convert(rgbColor2);


            //var labColor1 = new LabColor(50, 2.6772, -79.7751);
            //var labColor2 = new LabColor(50.0000, 0.0000, -82.7485);
            Console.WriteLine(labColor1.ToString());
            Console.WriteLine(labColor2.ToString());

            //var differenceCalculator = new CIEDE2000ColorDifference();
            //double difference = differenceCalculator.ComputeDifference(in labColor1, in labColor2); // 69.7388
            //Console.WriteLine($"diff:{difference}");

//docker run -d --hostname my-rabbit --name ecomm-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management

            /*
             * 
             * {
  a: 0.00526049995830391,
  b: -0.010408184525267927,
  L: 100
}
            {
  a: -47.649308298010396,
  b: -14.571279271078218,
  L: 90.82716767098302
}
            {"id":"a5ce113c-2108-4790-ab10-3305385d9061","status":"Completed","output":{"info":{"inputWidth":1350,"inputHeight":732,"outputWidth":1350,"outputHeight":732},"options":[{"url":"https://uploads.documents.cimpress.io/v1/uploads/ae265c38-b1e2-4426-8b42-a345b39dbd40~110?tenant=imagemind","colors":[{"r":0,"g":0,"b":0,"a":255},{"r":217,"g":55,"b":41,"a":255},{"r":175,"g":203,"b":201,"a":254},{"r":0,"g":0,"b":0,"a":0}],"confidence":-0.4836}],"colorPalettes":[[{"r":0,"g":0,"b":0,"a":13},{"r":206,"g":96,"b":85,"a":255}],[{"r":217,"g":55,"b":41,"a":255},{"r":175,"g":203,"b":201,"a":254},{"r":0,"g":0,"b":0,"a":13}],[{"r":0,"g":0,"b":0,"a":255},{"r":217,"g":55,"b":41,"a":255},{"r":175,"g":203,"b":201,"a":254},{"r":0,"g":0,"b":0,"a":0}],[{"r":0,"g":0,"b":0,"a":0},{"r":0,"g":0,"b":0,"a":255},{"r":53,"g":60,"b":61,"a":75},{"r":217,"g":55,"b":41,"a":255},{"r":175,"g":203,"b":201,"a":254}],[{"r":0,"g":0,"b":0,"a":0},{"r":0,"g":0,"b":0,"a":255},{"r":176,"g":204,"b":203,"a":255},{"r":53,"g":60,"b":61,"a":75},{"r":148,"g":168,"b":120,"a":238},{"r":217,"g":55,"b":41,"a":255}],[{"r":0,"g":0,"b":0,"a":0},{"r":0,"g":0,"b":0,"a":255},{"r":231,"g":39,"b":27,"a":255},{"r":205,"g":70,"b":53,"a":255},{"r":176,"g":204,"b":203,"a":255},{"r":53,"g":60,"b":61,"a":75},{"r":148,"g":168,"b":120,"a":238}],[{"r":0,"g":0,"b":0,"a":0},{"r":0,"g":0,"b":0,"a":255},{"r":152,"g":174,"b":109,"a":255},{"r":135,"g":153,"b":151,"a":190},{"r":231,"g":39,"b":27,"a":255},{"r":205,"g":70,"b":53,"a":255},{"r":176,"g":204,"b":203,"a":255},{"r":53,"g":60,"b":61,"a":75}],[{"r":0,"g":0,"b":0,"a":0},{"r":0,"g":0,"b":0,"a":255},{"r":82,"g":91,"b":93,"a":117},{"r":152,"g":174,"b":109,"a":255},{"r":48,"g":54,"b":55,"a":68},{"r":135,"g":153,"b":151,"a":190},{"r":231,"g":39,"b":27,"a":255},{"r":205,"g":70,"b":53,"a":255},{"r":176,"g":204,"b":203,"a":255}],[{"r":0,"g":0,"b":0,"a":0},{"r":0,"g":0,"b":0,"a":255},{"r":82,"g":91,"b":93,"a":117},{"r":152,"g":174,"b":109,"a":255},{"r":131,"g":167,"b":236,"a":255},{"r":48,"g":54,"b":55,"a":68},{"r":135,"g":153,"b":151,"a":190},{"r":231,"g":39,"b":27,"a":255},{"r":178,"g":206,"b":202,"a":255},{"r":205,"g":70,"b":53,"a":255}]],"url":"https://uploads.documents.cimpress.io/v1/uploads/ae265c38-b1e2-4426-8b42-a345b39dbd40~110?tenant=imagemind","colors":[{"r":0,"g":0,"b":0,"a":255},{"r":217,"g":55,"b":41,"a":255},{"r":175,"g":203,"b":201,"a":254},{"r":0,"g":0,"b":0,"a":0}],"confidence":-0.4836},"input":{"url":"https://uploads.documents.cimpress.io/v1/uploads/9b848555-c428-4ada-a345-8ab13a4972ef~110/original?tenant=cimpress-cn-uploads"}}
            diff({a:100, R: 255, G: 255, B:255},{a:100, R: 0, G: 254, B:255})

             * 
             * */
            //
            //ciede2000({L:50,a:2.6772,b:-79.7751},{L:50.0000,a:0.0000,b:-82.7485})
            //diff({ a: 100, R: 255, G: 255, B: 255},{ a: 100, R: 0, G: 254, B: 255})/
            //rgba_to_rgb({a: 0, R: 255, G: 100, B: 255})
        }
    }
}
