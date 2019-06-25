'use strict';

const gulp = require('gulp');
const sourcemaps = require('gulp-sourcemaps');
const sass = require('gulp-sass');

gulp.task('styles', [], () => {
  return gulp.src(['./**/*.scss', '!./node_modules/**/*', '!./**/variables.scss'], { base: "./" })
    .pipe(sourcemaps.init())
    .pipe(sass({ outputStyle: 'compact' }).on('error', sass.logError))
    .pipe(sourcemaps.mapSources(path => '../' + path.substr(path.indexOf('/'))))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('.'));
});

gulp.task('watch', () => {
    gulp.watch(['./**/*.scss', '!./**/variables.scss'], ['styles']);
});

gulp.task('build', ['styles', 'watch']);

gulp.task('default', ['build']);
