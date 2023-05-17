import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';
import { viteStaticCopy } from 'vite-plugin-static-copy'
import svgr from 'vite-plugin-svgr'

export default () =>
  defineConfig({
    plugins: [
      preact(),
      viteStaticCopy({
        targets: [
          {
            src: 'controls',
            dest: '',
          },
          {
            src: 'entity-list/columns',
            dest: './',
          },
        ],
      }),
      svgr({ exportAsDefault: true }),
    ],
    build: {
      rollupOptions: {
        external: [
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
