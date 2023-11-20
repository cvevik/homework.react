export const getMoviesByTitle = async (title: string) => {
    const moviesData = await fetch(`movies?title=${title}`);
    const movies: Movie[] = await moviesData.json();

    return movies;
}

export const getMovieById = async (id: string) => {
    const movieWithDescriptionData = await fetch(`movies/${id}`);
    const movieWithDescription: MovieWithDescription = await movieWithDescriptionData.json();

    return movieWithDescription;
}