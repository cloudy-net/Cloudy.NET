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
          src: 'form/form-field.js',
          dest: 'form/',
        },
        {
          src: 'data/state-manager.js',
          dest: 'data/',
        },
        {
          src: 'data/change-handlers/simple-change-handler.js',
          dest: 'data/change-handlers/',
        },
        {
          src: 'form/controls',
          dest: 'form/',
        },
        {
          src: 'util/*',
          dest: 'util/',
        },
        {
          src: 'components/*',
          dest: 'components/',
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
      '@preact-htm': preactLocation,
    }
  },
  server: {
    hmr: false
  },
  build: {
    rollupOptions: {
      external: [
        'data/change-handlers/simple-change-handler.js',
        'data/state-manager.js',
        'form/entity-context.js',
        'form/form-field.js',
        'media-picker/media-picker-menu.js',
        'components/*',
        'util/array-equals.js',
        'util/get-reference-value.js',
        'util/url-fetcher.js',
        'util/debounce.js',
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
