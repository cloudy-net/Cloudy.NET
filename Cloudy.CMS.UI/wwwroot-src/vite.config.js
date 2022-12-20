import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';

export default defineConfig({
  plugins: [preact()],
  resolve: {
    alias: {
      '@preact-htm': './preact-htm/standalone.module.js'
    }
  },
  server: {
    fs: {
      strict: false
    }
  },
  build: {
    rollupOptions: {
      external: [
        './preact-htm/standalone.module.js'
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
