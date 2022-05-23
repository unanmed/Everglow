﻿namespace Everglow.Sources.Modules.ZY.Commons.Core.Draw;

internal static class DrawUtils
{
    public static DrawState GetState(this GraphicsDevice graphicsDevice)
    {
        return new DrawState(graphicsDevice.BlendState, graphicsDevice.SamplerStates[0], graphicsDevice.DepthStencilState, graphicsDevice.RasterizerState);
    }

    public static void SetState(this GraphicsDevice graphicsDevice, DrawState drawState) => drawState.SetState(graphicsDevice);

    public static void Begin(this SpriteBatch spriteBatch, SpriteSortMode spriteSortMode, DrawState drawState)
    {
        spriteBatch.Begin(spriteSortMode, drawState.blendState, drawState.samplerState, drawState.depthStencilState, drawState.rasterizerState);
    }
    public static void Begin(this SpriteBatch spriteBatch, SpriteSortMode spriteSortMode, DrawState drawState, Matrix matrix)
    {
        spriteBatch.Begin(spriteSortMode, drawState.blendState, drawState.samplerState, drawState.depthStencilState, drawState.rasterizerState, null, matrix);
    }
}
