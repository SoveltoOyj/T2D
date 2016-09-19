using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
//using ZXing;
using ZXing.Common;
using ZXing.Mobile;
//using ZXing;

//using BarcodeScanControl;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace QRTest1
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page, INotifyPropertyChanged
	{

		private StorageFile storeFile;
		private IRandomAccessStream stream;

		UIElement customOverlayElement = null;
		MobileBarcodeScanner scanner;


		public MainPage()
		{
			this.InitializeComponent();
			//DataContext = this;
			ScannedText = "starting...";
			scanner = new MobileBarcodeScanner(this.Dispatcher);
			scanner.Dispatcher = this.Dispatcher;
		}

		private async void x()
		{
			//await BarcodeScanner.GetDefaultAsync();
			//-----


			var scanner = new ZXing.Mobile.MobileBarcodeScanner(this.Dispatcher);
			MobileBarcodeScanningOptions opts = new MobileBarcodeScanningOptions();
			opts.AutoRotate = true;
			//opts.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE };

			var result = await scanner.Scan(opts);

			if (result != null)
			{
				Debug.WriteLine("Scanned Barcode: " + result.Text);
				//tbData.Text = result.Text;
			}

		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			x();
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			string s = _scannedText;
			ScannedText = "Found: " + s;
		}

		//private async void button2_Click(object sender, RoutedEventArgs e) {
		////private async void captureBtn_Click(object sender, RoutedEventArgs e) {
		//CameraCaptureUI capture = new CameraCaptureUI();
		//capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
		//capture.PhotoSettings.CroppedSizeInPixels = new Windows.Foundation.Size(200, 200);
		////capture.PhotoSettings.CroppedAspectRatio = new Size(1, 1);
		//capture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
		//storeFile = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);
		//if (storeFile != null) {
		//    BitmapImage bimage = new BitmapImage();
		//    stream = await storeFile.OpenAsync(FileAccessMode.Read); ;
		//    bimage.SetSource(stream);
		//    captureImage.Source = bimage;


		//    IBarcodeReader reader = new ZXing.BarcodeReader();
		//    reader.Options.TryHarder = true;
		//    reader.Options.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE };



		//    byte[] buffer;
		//    using (var dataReader = new DataReader(stream.GetInputStreamAt(0))) {
		//        await dataReader.LoadAsync((uint)stream.Size);
		//        buffer = new byte[(int)stream.Size];
		//        dataReader.ReadBytes(buffer);
		//    }

		//    LuminanceSource source = new RGBLuminanceSource(buffer, bimage.PixelWidth, bimage.PixelHeight, RGBLuminanceSource.BitmapFormat.Gray8);
		//    var binarizer = new HybridBinarizer(source);
		//    var binBitmap = new BinaryBitmap(binarizer);

		//    //var result = reader.Decode(binBitmap, bimage.PixelWidth, bimage.PixelHeight, RGBLuminanceSource.BitmapFormat.Unknown);

		//    var result = reader.Decode(source);

		//    //var result = reader.Decode(buffer, bimage.PixelWidth, bimage.PixelHeight, RGBLuminanceSource.BitmapFormat.Unknown);
		//    //var result = reader.Decode(buffer, bimage.PixelWidth, bimage.PixelHeight, RGBLuminanceSource.BitmapFormat.Gray8 );


		//    //var result = reader.Decode(bimage);
		//    // do something with the result
		//    if (result != null) {
		//        //txtDecoderType.Text = result.BarcodeFormat.ToString();
		//        tbData.Text = result.Text;
		//    }
		//    else {
		//        tbData.Text = "N/A";
		//    }
		//}
		//}

		////private async void saveBtn_Click(object sender, RoutedEventArgs e) {
		//    try {

		//        FileSavePicker fs = new FileSavePicker();
		//        fs.FileTypeChoices.Add("Image", new List<string>() { ".jpeg" });
		//        fs.DefaultFileExtension = ".jpeg";
		//        fs.SuggestedFileName = "Image" + DateTime.Today.ToString();
		//        fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
		//        fs.SuggestedSaveFile = storeFile;
		//        // Saving the file
		//        var s = await fs.PickSaveFileAsync();
		//        if (s != null) {
		//            using (var dataReader = new DataReader(stream.GetInputStreamAt(0))) {
		//                await dataReader.LoadAsync((uint)stream.Size);
		//                byte[] buffer = new byte[(int)stream.Size];
		//                dataReader.ReadBytes(buffer);
		//                await FileIO.WriteBytesAsync(s, buffer);
		//            }
		//        }
		//    }
		//    catch (Exception ex) {
		//        var messageDialog = new MessageDialog("Unable to save now.");
		//        await messageDialog.ShowAsync();
		//    }
		//}

		private void HandleBarcodeButtonClick(object sender, RoutedEventArgs e)
		{
			scanner.UseCustomOverlay = false;
			scanner.TopText = "Hold camera up to barcode";
			scanner.BottomText = "Camera will automatically scan barcode\r\n\r\nPress the 'Back' button to Cancel";

			Action<string> showText = j => ScannedText = j;

			scanner.Scan().ContinueWith(t =>
			{
				if (t.Result != null)
				{
					HandleScanResult(t.Result);
				}
			});
		}

		async void HandleScanResult(ZXing.Result result)
		{
			string msg = "";

			if (result != null && !string.IsNullOrEmpty(result.Text))
			{
				msg = GetBasicDataFromInventory(FromTagToId(result.Text));
			}
			else
				msg = "Scanning Canceled!";
			await ShowResult(msg);
		}

		async Task ShowResult(string text)
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
			{
				txtID.Text = text;
				var dialog = new MessageDialog(text);
				await dialog.ShowAsync();
			});
		}


		/// <summary>
		/// Returns identifier from a tag string.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		private string FromTagToId(string tag)
		{
			// barcode has identifier
			return tag;
		}

		private string GetBasicDataFromInventory(string identifier)
		{
			return $"Identifier:{identifier}\nTitle:Suitcase 1\nLocation:Getting location from a container...";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private string _scannedText;
		public string ScannedText
		{
			get { return _scannedText; }
			set
			{
				_scannedText = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("ScannedText"));
				}
				//Debug.WriteLine(value, "ScannedText-property value: ");
			}
		}

	}
}

