import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';
import path from 'path';

const preactLocation = path.join(path.resolve(__dirname), './preact-htm/standalone.module.js');

export default defineConfig({
  plugins: [preact()],
  resolve: {
    alias: {
      '@preact-htm': preactLocation
    }
  },
  build: {
    rollupOptions: {
      external: [
        preactLocation
      ],
      output: {
        entryFileNames: '[name].bundle.js',
        assetFileNames: '[name].[ext]',
        chunkFileNames: '[name].[ext]'
      }
    },
    sourcemap: true,
    outDir: '../wwwroot'
  }
});
