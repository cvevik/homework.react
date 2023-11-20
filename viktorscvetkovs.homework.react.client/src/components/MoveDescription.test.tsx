import { render, screen, waitFor } from '@testing-library/react';
import * as movieService from "../services/movieService";
import MovieDescription from "./MovieDescription";

describe('MovieDescription', () => {
    test('Loads proper data', async () => {
        const testMovieWithDescription: Partial<MovieWithDescription> = {
            genre: 'testGenre',
            imdbRating: 'testRating',
            country: 'testCountry',
            language: 'testLanguage',
            runtime: 'testRuntime',
            plot: 'testPlot'
        };
        const testMovie: Movie = {
            title: 'testTitle',
            year: 'testYear',
            poster: 'testPosterUrl',
            imdbID: 'testId',
            type: 'testType'
        };
        vi.spyOn(movieService, 'searchMovieById')
            .mockImplementation(async () => testMovieWithDescription as MovieWithDescription);
        render(<MovieDescription movie={testMovie} />);

        await waitFor(async () => {
            const img = await screen.findByAltText('Movie poster');
            expect(img.getAttribute('src'))
                .toBe(testMovie.poster);
            expect(screen.getByTestId('title').textContent)
                .toBe(testMovie.title);
            expect(screen.getByTestId('year').textContent)
                .toBe(testMovie.year);

            expect(screen.getByTestId('genre').textContent)
                .toBe(testMovieWithDescription.genre);
            expect(screen.getByTestId('rating').textContent)
                .toBe(testMovieWithDescription.imdbRating);
            expect(screen.getByTestId('country').textContent)
                .toBe(testMovieWithDescription.country);
            expect(screen.getByTestId('language').textContent)
                .toBe(testMovieWithDescription.language);
            expect(screen.getByTestId('time').textContent)
                .toBe(testMovieWithDescription.runtime);
            expect(screen.getByTestId('description').textContent)
                .toBe(testMovieWithDescription.plot);
        });
    });
});