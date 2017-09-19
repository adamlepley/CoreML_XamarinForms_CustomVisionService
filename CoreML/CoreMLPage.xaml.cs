using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;

namespace CoreML
{
    public partial class CoreMLPage : ContentPage
    {
        public CoreMLPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // taking a picture
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(options: new StoreCameraMediaOptions());

            if (file == null)
            {
                return;
            }

            this.image.Source = ImageSource.FromStream(() => file.GetStream());

            using (var fileStream = file.GetStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    var d = DependencyService.Get<Services.IDetectService>();
                    var result = await d.DetectAsync(memoryStream.ToArray());
                    await this.DisplayAlert(title: "result", message: result, cancel: "OK");
                }
            }
        }
    }
}
