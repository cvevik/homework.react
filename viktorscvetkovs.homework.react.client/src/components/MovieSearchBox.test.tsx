import MovieSearchBox from './MovieSearchBox';
import { render, screen, waitFor } from '@testing-library/react';
import { userEvent } from '@testing-library/user-event';
import * as movieService from '../services/movieService';

describe('MovieSearchBox', () => {
    test('Autocomplete click opens popper', async () => {
        render(<MovieSearchBox onSelectMovie={() => { } } />)
        await userEvent.click(await screen.findByLabelText(/Search movie by title/i));

        const loadingElement = screen.getByText(/Loading/i);
        expect(loadingElement).toBeInTheDocument();
    });

    test("Autocomplete click loads initial data and can be selected", async () => {
        const testData = [
            {
                "title": "Spider-Man",
                "year": "2002",
                "imdbID": "tt0145487",
                "type": "movie",
                "poster": "https://m.media-amazon.com/images/M/MV5BZDEyN2NhMjgtMjdhNi00MmNlLWE5YTgtZGE4MzNjMTRlMGEwXkEyXkFqcGdeQXVyNDUyOTg3Njg@._V1_SX300.jpg"
            },
            {
                "title": "Spider-Man: No Way Home",
                "year": "2021",
                "imdbID": "tt10872600",
                "type": "movie",
                "poster": "https://m.media-amazon.com/images/M/MV5BZWMyYzFjYTYtNTRjYi00OGExLWE2YzgtOGRmYjAxZTU3NzBiXkEyXkFqcGdeQXVyMzQ0MzA0NTM@._V1_SX300.jpg"
            }
        ] as Movie[];
        vi.spyOn(movieService, 'getMoviesByTitle')
            .mockImplementation(async () => testData);
        const mockOnSelectMovie = vi.fn();
        render(<MovieSearchBox onSelectMovie={mockOnSelectMovie} />)
        await userEvent.click(screen.getByLabelText(/Search movie by title/i));

        const loadingElement = screen.getByText(/Loading/i);
        expect(loadingElement).toBeInTheDocument();

        await waitFor(async () => {
            const options = screen.getAllByRole('option');
            expect(options.length).toBe(2);
            expect(options[0].querySelector('img')?.src).toBe(testData[0].poster);
            
            const firstOptionMainText = options[0]
                .querySelector('[data-testid="movie-option-main-text"]')?.textContent
            expect(firstOptionMainText).toBe(`${testData[0].title} (${testData[0].year})`);

            const firstOptionSecondaryText = options[0]
                .querySelector('[data-testid="movie-option-secondary-text"]')?.textContent
            expect(firstOptionSecondaryText).toBe(testData[0].type);

            await userEvent.click(options[0]);
            expect(mockOnSelectMovie).toBeCalled();
        });
    });
});