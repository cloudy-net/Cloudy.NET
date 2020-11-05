const gulp = require('gulp');
const sass = require('gulp-sass');
const del = require('del');

gulp.task('styles', () => {
    return gulp.src([
      'wwwroot/**/*.scss',
      '!wwwroot/variables.scss',
    ])
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/'));
});

gulp.task('clean', () => {
    return del([
        'wwwroot/**/*.css',
    ]);
});

gulp.task('default', gulp.series(['clean', 'styles']));

gulp.task('watch', () => {
  gulp.watch('wwwroot/**/*.scss', (done) => {
      gulp.series(['clean', 'styles'])(done);
  });
});
