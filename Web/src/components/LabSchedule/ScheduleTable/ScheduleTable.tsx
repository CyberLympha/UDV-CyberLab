import React from 'react';
import { LabReservation } from '.../../../api';
import style from './ScheduleTable.module.scss';

interface ScheduleTableProps {
  scheduleData: LabReservation[];
  selectedWeek: Date;
  handleReservationClick: (reservation: LabReservation) => void;
}

export const ScheduleTable: React.FC<ScheduleTableProps> = ({
  scheduleData,
  selectedWeek,
  handleReservationClick,
}) => {
  const renderScheduleTable = () => {
    const daysOfWeek = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'];
    const timeSlots = ['7:00', '8:00', '9:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00'];
  
    const startDate = new Date(selectedWeek);
    startDate.setDate(startDate.getDate() - startDate.getDay());

    const tableHeader = (
      <tr>
        <th></th>
        {Array.from({ length: 7 }, (_, index) => {
          const currentDate = new Date(startDate);
          currentDate.setDate(currentDate.getDate() + index);
          return <th key={index}>{`${daysOfWeek[index]} ${currentDate.getDate()}.${currentDate.getMonth() + 1}`}</th>;
        })}
      </tr>
    );
  
    const tableRows = timeSlots.map((timeSlot, rowIndex) => {
      return (
        <tr key={rowIndex}>
          <td>{timeSlot}</td>
          {Array.from({ length: 7 }, (_, index) => {
            const currentDate = new Date(startDate);
            currentDate.setDate(currentDate.getDate() + index);
  
            const reservations = scheduleData.filter((reservation) => {
              const reservationStartDate = new Date(reservation.timeStart);
              const reservationEndDate = new Date(reservation.timeEnd);
              return (
                reservationStartDate.getDate() === currentDate.getDate() &&
                reservationStartDate.getMonth() === currentDate.getMonth() &&
                reservationStartDate.getFullYear() === currentDate.getFullYear() &&
                reservationStartDate.getHours().toString() === timeSlot.split(':')[0]
              );
            });
  
            return (
              <td key={index}>
                {reservations.length > 0 ? (
                  reservations.map((reservation, resIndex) => (
                    <div
                      key={resIndex}
                      className={style.reservationCell}
                    >
                      <div
                        className={style.reservationCard}
                        onClick={() => handleReservationClick(reservation)}
                      >
                        {`${reservation.timeStart.split('T')[1].split(':')[0]}:${reservation.timeStart.split('T')[1].split(':')[1]} -
                      ${reservation.timeEnd.split('T')[1].split(':')[0]}:${reservation.timeEnd.split('T')[1].split(':')[1]}`}
                      <br/>
                      {`${reservation.theme}`}
                      </div>
                    </div>
                  ))
                ) : (
                  <div className={style.emptyCell}></div>
                )}
              </td>
            );
          })}
        </tr>
      );
    });
  
    return (
      <table className={style.scheduleTable}>
        <thead>{tableHeader}</thead>
        <tbody>{tableRows}</tbody>
      </table>
    );
  };

  return (
    <div className={style.scheduleTable}>
      {renderScheduleTable()}
    </div>
  );
};
