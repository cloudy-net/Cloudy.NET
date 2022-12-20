import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';
import fs from 'fs';
import alias from '@rollup/plugin-alias';

export default defineConfig({
  plugins: [preact()],
  build: {
    rollupOptions: {
      external: [
        '/preact-htm/standalone.module.js'
      ],
      output: {
        entryFileNames: '[name].bundle.js',
        assetFileNames: '[name].[ext]',
        chunkFileNames: '[name].[ext]'
      },
      plugins: [
        {
          name: 'copy-files',
          generateBundle: () => fs.copyFileSync('node_modules/htm/preact/standalone.module.js', '../wwwroot/preact-htm/standalone.module.js')
        },
        alias({
          entries: [
            { find: '@preact-htm', replacement: './preact-htm/standalone.module.js' }
          ]
        })
      ]
    },
    sourcemap: true,
    outDir: '../wwwroot'
  }
});
