const JobAdvertisementDetails=({ad, onEdit, onDelete, onCheckApplications})=>{
    return(
        <>
          <p className="text-lg font-semibold">
            <strong>Naslov oglasa: </strong>
            {ad.title}
          </p>
          <p>
            <strong>Kratak opis: </strong>
            {ad.shortDescription}
          </p>
          <p>
            <strong>Grad: </strong>
            {ad.city}
          </p>
          <p>
            <strong>Ulica: </strong>
            {ad.street}
          </p>
          <p>
            <strong>Kucni broj: </strong>
            {ad.homeNumber}
          </p>
          <p>
            <strong>Datum obavljanja posla: </strong>
            {ad.dateOfExecution}
          </p>
          <p>
            <strong>Trajanje posla: </strong>
            {ad.workDuration}
          </p>
          <p>
            <strong>Naknada po satu: </strong>
            {ad.jobSalary}
          </p>
          <p>
            <strong>Tip posla: </strong>
            {ad.jobType}
          </p>

          <button
            onClick={onEdit}
            className="mt-2 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 w-full"
          >
            Izmenite oglas
          </button>

          <button
            onClick={onCheckApplications}
            className="mt-2 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 w-full"
          >
            Pregledajte prijave
          </button>
          <button
            onClick={onDelete}
            className="mt-2 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 w-full"
          >
            Obriši oglas
          </button>
        </>
    )

}

export default JobAdvertisementDetails;