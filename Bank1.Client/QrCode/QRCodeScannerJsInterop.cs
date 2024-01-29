﻿using Microsoft.JSInterop;

namespace Bank1.Client.QrCode
{
    public class QRCodeScannerJsInterop : IAsyncDisposable
    {
        private static DateTime? _lastScannedValueDateTime;
        private static int _scanInterval = 2000;

        private static Action<string>? _onQrCodeScanAction;
        private static Action<string>? _onCameraPermissionFailedAction;

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public QRCodeScannerJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/qrCodeScannerJsInterop.js").AsTask());
        }

        public async ValueTask Init(Action<string> onQrCodeScanAction, bool useFrontCamera = false)
        {
            _onQrCodeScanAction = onQrCodeScanAction;

            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("Scanner.Init", new object[1] { useFrontCamera });
        }

        public async ValueTask Init(Action<string> onQrCodeScanAction, Action<string> onCameraPermissionFailedAction
            , bool useFrontCamera = false)
        {
            _onQrCodeScanAction = onQrCodeScanAction;
            _onCameraPermissionFailedAction = onCameraPermissionFailedAction;

            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("Scanner.Init", new object[1] { useFrontCamera });
        }

        public async ValueTask StopRecording()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.InvokeVoidAsync("Scanner.Stop");
            }
        }


        [JSInvokable]
        public static Task<string> ManageErrorJsCallBack(string value)
        {
            Console.WriteLine(value);

            _onCameraPermissionFailedAction?.Invoke(value);

            return Task.FromResult("retour");
        }


        [JSInvokable]
        public static Task<string> QRCodeJsCallBack(string value)
        {
            if (_lastScannedValueDateTime == null)
            {
                _lastScannedValueDateTime = DateTime.Now;
                DoSomethingAboutThisQRCode(value);
            }

            var maxDate = DateTime.Now.AddMilliseconds(-_scanInterval);
            if (_lastScannedValueDateTime < maxDate)
            {
                _lastScannedValueDateTime = DateTime.Now;
                DoSomethingAboutThisQRCode(value);
            }

            return Task.FromResult("retour"); 
        }

        public static void DoSomethingAboutThisQRCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                _onQrCodeScanAction?.Invoke(code);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
