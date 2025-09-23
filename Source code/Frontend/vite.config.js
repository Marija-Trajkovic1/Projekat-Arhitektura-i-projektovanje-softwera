import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(),
     tailwindcss()
  ],
  server:{
    proxy:{
      '/api':{
        target:'http://localhost:7260',
        secure:false
      },
      '^/pingauth': {
                target: 'https://localhost:7260/',
                secure: false
            },
    },
    port: 5173,
    historyApiFallback: true
  }
})
