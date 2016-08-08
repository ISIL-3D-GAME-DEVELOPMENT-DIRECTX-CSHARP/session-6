using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D11;

using Device = SharpDX.Direct3D11.Device;

namespace Sesion2_Lab01 {
    public class NTexture2D {

        private Device mDevice;
        private Texture2D mTexture;
        private ShaderResourceView mTextureResource;

        private SamplerState mSamplerState;
        private SamplerStateDescription mSamplerStateDescription;

        private int mWidth;
        private int mHeight;

        public int Width    { get { return mWidth; } }
        public int Height   { get { return mHeight; } }

        public SamplerState SamplerState            { get { return mSamplerState; } }
        public ShaderResourceView TextureResource   { get { return mTextureResource; } } 

        public NTexture2D(Device device) {
            mDevice = device;

            // ahora creamos el descriptor del sampler, para que el GPU sepa
            // como dibujarlo en pantalla
            mSamplerStateDescription.Filter = Filter.MinMagMipLinear;
            mSamplerStateDescription.AddressU = TextureAddressMode.Clamp;
            mSamplerStateDescription.AddressV = TextureAddressMode.Clamp;
            mSamplerStateDescription.AddressW = TextureAddressMode.Clamp;
            mSamplerStateDescription.BorderColor = SharpDX.Color.Transparent;
            mSamplerStateDescription.ComparisonFunction = Comparison.Never;
            mSamplerStateDescription.MaximumAnisotropy = 1;
            mSamplerStateDescription.MipLodBias = 0;
            mSamplerStateDescription.MinimumLod = -float.MaxValue;
            mSamplerStateDescription.MaximumLod = -float.MaxValue;
        }

        public void Load(string path) {
            mTexture = Texture2D.FromFile<Texture2D>(mDevice, path);

            mWidth = mTexture.Description.Width;
            mHeight = mTexture.Description.Height;

            mSamplerState = new SamplerState(mDevice, ref mSamplerStateDescription);

            mTextureResource = new ShaderResourceView(mDevice, mTexture);
        }
    }
}
