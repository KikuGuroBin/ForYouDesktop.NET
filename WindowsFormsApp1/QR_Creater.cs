﻿using System.Drawing;

using ThoughtWorks.QRCode.Codec;

namespace WindowsFormsApp1
{
    class QR_Creater
    {
        public  QR_Creater()
        {
        }
        public Image CrGet(string qr)
        {
            QRCodeEncoder qrEnc = new QRCodeEncoder();

            // エンコードは英数字
            qrEnc.QRCodeEncodeMode =
                QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;

            // エラー訂正はM
            qrEnc.QRCodeErrorCorrect =
                 QRCodeEncoder.ERROR_CORRECTION.M;

            qrEnc.QRCodeVersion = 7; // バージョン（1～40）

            qrEnc.QRCodeScale = 4; // 1セルのピクセル数

            Image image;
            // 文字列を指定してQRコードを生成
            image = qrEnc.Encode(qr);
            return image;
        }
    }
}
