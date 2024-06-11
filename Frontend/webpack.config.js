const path = require('path'); // Импортируем модуль "path" для работы с путями файлов
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

const fs = require('fs');

function generateHtmlPluginsPerFolder(templateDir) {
    const folderName = templateDir.split('/').pop();
    const templateFiles = fs.readdirSync(path.resolve(__dirname, templateDir));
    return templateFiles.filter(item => item.endsWith('.html')).map(item => {
        return new HtmlWebpackPlugin({
            filename: path.join(`pages/${folderName}`, item),
            template: path.resolve(__dirname, `${templateDir}/${item}`),
            chunks: [`${folderName}`, 'auth'],
            scriptLoading: 'blocking',
            inject: 'head'
        });
    });
}

const htmlPluginsPerAuth = generateHtmlPluginsPerFolder('./src/pages/auth');
const htmlPluginsPerTeacher = generateHtmlPluginsPerFolder('./src/pages/teacher');

module.exports = {
  entry: {
    entry: './src/index.js', // Точка входа для сборки проекта
    auth: ['./src/scripts/api/auth/auth.js', './src/scripts/api/common/hello.js', './src/scripts/dom/userInfo.js'],
    teacher: ['./src/scripts/api/teacher/teacherLabs.js']
  },

  output: {
    filename: '[name].bundle.js', // Имя выходного файла сборки
    path: path.resolve(__dirname, 'dist'), // Путь для выходного файла сборки
  },

  module: {
    rules: [
      {
        test: /\.css$/, // Регулярное выражение для обработки файлов с расширением .css
        use: [MiniCssExtractPlugin.loader, 'css-loader'], // Загрузчики, используемые для обработки CSS-файлов
      },
    ],
  },
  
  plugins: [
    new webpack.DefinePlugin({
      'process.env.AUTH_URL': JSON.stringify(
        process.env.NODE_ENV === 'production'
              ? 'http://158.160.91.137'
              :  process.env.NODE_ENV === 'deploy-private'
              ? 'http://localhost:8081'
              : 'https://localhost:7182'
      ),
      'process.env.LABS_URL': JSON.stringify(
        process.env.NODE_ENV === 'production'
              ? 'http://158.160.91.137'
              :  process.env.NODE_ENV === 'deploy-private'
              ? 'http://localhost:8080'
              : 'https://localhost:7218'
    ),
  }),
    new HtmlWebpackPlugin({
      filename: 'index.html',
      template: './src/index.html',
      chunks: ['entry']
    }),
    new HtmlWebpackPlugin({
        filename: 'pages/AuthorisedPage.html',
        template: './src/pages/AuthorisedPage.html',
        chunks: ['auth'],
        scriptLoading: 'blocking',
        inject: 'head'
      }),
      new HtmlWebpackPlugin({
        filename: 'pages/HelloPage.html',
        template: './src/pages/HelloPage.html',
        chunks: ['auth'],
        scriptLoading: 'blocking',
        inject: 'head'
      }),
    ...htmlPluginsPerAuth,
    ...htmlPluginsPerTeacher,
    new MiniCssExtractPlugin({
        filename: 'styles/styles.css',
    }),
  ],

  devServer: {
    static: {
      directory: path.join(__dirname, 'dist'), // Каталог для статики
    },
    open: true, // Автоматически открывать браузер
    port: 8082
  },

  mode: 'development', // Режим сборки
};