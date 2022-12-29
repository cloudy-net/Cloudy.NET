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
          src: 'form/entity-context.js',
          dest: 'form/',
        },
        {
          src: 'data/state-manager.js',
          dest: 'data/',
        },
        {
          src: 'form/controls',
          dest: 'form/',
        },
        {
          src: 'node_modules/htm/preact/standalone.module.js',
          dest: 'preact-htm/',
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
        'data/state-manager.js',
        'form/entity-context.js',
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
