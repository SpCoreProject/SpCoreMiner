var input = document.getElementById("qrinput");
const canvasdevice = document.getElementById("qrdevice");
const canvaswallet = document.getElementById("qrwallet");
const canvascard = document.getElementById("qrcard");

const canvasdevicepopup = document.getElementById("qrdevicepopup");
const canvaswalletpopup = document.getElementById("qrwalletpopup");
const canvascardpopup = document.getElementById("qrcardpopup");

const createDeviceQR = v => {
    // https://github.com/neocotic/qrious
    return new QRious({
        element: canvasdevice,
        value: v,
        size: 120,
        backgroundAlpha: 0,
        foreground: "white"
    });

};

const createCardQR = v => {
    // https://github.com/neocotic/qrious
    return new QRious({
        element: canvascard,
        value: v,
        size: 120,
        backgroundAlpha: 0,
        foreground: "white"
    });

};

const createWalletQR = v => {
    // https://github.com/neocotic/qrious
    return new QRious({
        element: canvaswallet,
        value: v,
        size: 120,
        backgroundAlpha: 0,
        foreground: "white"
    });

};



const createDevicepopupQR = v => {
    // https://github.com/neocotic/qrious
    return new QRious({
        element: canvasdevicepopup,
        value: v,
        size: 120,
        backgroundAlpha: 0,
        foreground: "white"
    });

};

const createCardpopupQR = v => {
    // https://github.com/neocotic/qrious
    return new QRious({
        element: canvascardpopup,
        value: v,
        size: 120,
        backgroundAlpha: 0,
        foreground: "white"
    });

};

const createWalletpopupQR = v => {
    // https://github.com/neocotic/qrious
    return new QRious({
        element: canvaswalletpopup,
        value: v,
        size: 120,
        backgroundAlpha: 0,
        foreground: "white"
    });

};



//const qrdevice = createDeviceQR("bc1pauu63gj5gqpy2l9hh3nwvrr0ac56vhkj3jx5uas0esms9");
//const qrwallet = createWalletQR("y2l9hh3nwvrr0ac56bc1pauu63gj5gqpvhkj3jx5uas0esms9");
//const qrdevicepopup = createDevicepopupQR("bc1pauu63gj5gqpy2l9hh3nwvrr0ac56vhkj3jx5uas0esms9");
//const qrwalletpopup = createWalletpopupQR("y2l9hh3nwvrr0ac56bc1pauu63gj5gqpvhkj3jx5uas0esms9");
//const qrcard = createCardQR("vrr0ac56vhkj3jx5uas0esms9bc1pauu63gj5gqpy2l9hh3nw");
//const qrcardpopup = createCardpopupQR("vrr0ac56vhkj3jx5uas0esms9bc1pauu63gj5gqpy2l9hh3nw");



//const qr = createDeviceQR(input.value);

//input.addEventListener("input", () => {
//    const qr = createDeviceQR(input.value);
//});

//bc1pauu63gj5gqpy2l9hh3nj68dgdczswg83wvrr0ac56vhkj3jx5uas0esms9