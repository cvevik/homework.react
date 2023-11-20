import * as React from 'react';
import MovieSearchBox from './components/MovieSearchBox';
import MovieDescription from './components/MovieDescription';

function App() {
    const [movie, setMovie] = React.useState<Movie | null>(null);

    const onSelectMovie = (movie: Movie | null): void => {
        setMovie(movie);
    };

    return (
        <React.Fragment>
            <h1>Viktors Cvetkovs React homework</h1>
            <MovieSearchBox onSelectMovie={onSelectMovie} />
            <br />
            <MovieDescription movie={movie} />
        </React.Fragment>
    );
}

export default App;