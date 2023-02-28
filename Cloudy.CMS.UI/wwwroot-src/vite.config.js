import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';
import path from 'path';
import mkcert from 'vite-plugin-mkcert';
import { viteStaticCopy } from 'vite-plugin-static-copy'

export default ({ mode }) => {
  return defineConfig({
    base: mode === 'development' ? '/Admin/' : '/',
    plugins: [
      mkcert(),
      preact(),
      viteStaticCopy({
        targets: [
          {
            src: 'form/controls',
            dest: 'form/',
          },
        ],
      }),
    ],
    resolve: {
      alias: {
        '@src': path.resolve(__dirname, './')
      }
    },
    server: {
      https: true,
      port: 5001,
      proxy: {
        '^/Admin/api/.*': {
          changeOrigin: true,
          target: 'http://localhost:5000/'
        }
      },
    },
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
      outDir: 'dist'
    }
  });
};
