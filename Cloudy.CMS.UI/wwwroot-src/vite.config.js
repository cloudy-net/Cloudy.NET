import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';
import path from 'path';
import { viteStaticCopy } from 'vite-plugin-static-copy'

const preactLocation = path.join(path.resolve(__dirname), './preact-htm/standalone.module.js');

export default defineConfig({
  plugins: [
    preact(),
    viteStaticCopy({
      targets: [
        {
          src: 'change/change-tracker.js',
          dest: 'change/change-tracker.js',
        },
        {
          src: 'node_modules/htm/preact/standalone.module.js',
          dest: 'preact-htm/standalone.module.js',
        },
      ],
    }),
  ],
  resolve: {
    alias: {
      '@preact-htm': preactLocation
    }
  },
  build: {
    rollupOptions: {
      external: [
        'change/change-tracker.js',
        preactLocation,
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
