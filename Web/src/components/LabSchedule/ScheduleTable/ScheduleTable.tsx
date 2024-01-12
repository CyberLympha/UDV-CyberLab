import { LabReservation } from '.../../../api';

import style from './ScheduleTable.module.scss';

interface ScheduleTableProps {
  scheduleData: LabReservation[];
  selectedWeek: Date;
  handleReservationClick: (reservation: LabReservation) => void;
}

interface LabReservationCard {
  reservation: LabReservation;
  isStartingNextRow: boolean;
  top: number;
  bottom: number;
  height: number;
  topCellIndex: number;
  bottomCellIndex: number;
}

export const ScheduleTable: React.FC<ScheduleTableProps> = ({
  scheduleData,
  selectedWeek,
  handleReservationClick,
}) => {
  const renderScheduleTable = () => {
    const daysOfWeek = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'];
    const timeSlots = [
      '06:50 - 08:20',
      '08:30 - 10:00',
      '10:15 - 11:45',
      '12:00 - 13:30',
      '14:15 - 15:45',
      '16:00 - 17:30',
      '17:40 - 19:10',
      '19:15 - 20:45',
      '20:50 - 22:50',
    ];

    const startDate = new Date(selectedWeek);
    startDate.setDate(startDate.getDate() - startDate.getDay() + 1);

    const isCurrentDay = (date: Date) => {
      const currentDate = new Date();
      return (
        date.getDate() === currentDate.getDate() &&
        date.getMonth() === currentDate.getMonth() &&
        date.getFullYear() === currentDate.getFullYear()
      );
    };

    const tableHeader = (
      <tr>
        <th></th>
        {Array.from({ length: 7 }, (_, index) => {
          const currentDate = new Date(startDate);
          currentDate.setDate(currentDate.getDate() + index);
          const day = currentDate.getDate().toString().padStart(2, '0');
          const month = (currentDate.getMonth() + 1).toString().padStart(2, '0');
          const currentDayCellStyle = {
            background: isCurrentDay(currentDate) ? "#EFF6FF" : "white"
          };
          return <th key={index} className={style.daySlot} style={currentDayCellStyle}>
            <span className={style.dayName}>{`${daysOfWeek[index]}`}</span>
            <br />
            <span className={style.date}>{`${day}.${month}`}</span>
          </th>;
        })}
      </tr>
    );

    const tableRows = timeSlots.map((timeSlot, rowIndex) => {
      const [startTime, endTime] = timeSlot.split(' - ');
      const cellHeight = 40;
      const cellPadding = 10;
      const border = 1;
      const cellFullHeight = cellHeight + cellPadding * 2 + border * 0.8;
      const calculatePixelsPerMinute = (timeEnd: Date, timeStart: Date) => {
        return cellFullHeight / (timeEnd.getTime() - timeStart.getTime());
      }

      return (
        <tr key={rowIndex}>
          <td className={style.timeSlot}>{timeSlot}</td>
          {Array.from({ length: 7 }, (_, index) => {
            const currentDate = new Date(startDate);
            currentDate.setDate(currentDate.getDate() + index);

            const cellStartTime = new Date(currentDate);
            cellStartTime.setHours(parseInt(startTime.split(':')[0]), parseInt(startTime.split(':')[1]), 0, 0);
            const cellEndTime = new Date(currentDate);
            cellEndTime.setHours(parseInt(endTime.split(':')[0]), parseInt(endTime.split(':')[1]), 0, 0);

            const labReservationCards = [];

            for (const reservation of scheduleData) {
              const resStartTime = new Date(reservation.timeStart);
              const resEndTime = new Date(reservation.timeEnd);

              if (resStartTime >= cellStartTime && resStartTime <= cellEndTime) {
                const labReservationCard: LabReservationCard = {
                  reservation: reservation,
                  isStartingNextRow: false,
                  top: 0,
                  bottom: cellFullHeight,
                  height: 0,
                  topCellIndex: 0,
                  bottomCellIndex: 0,
                };

                for (let i = 0; i < timeSlots.length; i++) {
                  const [slotStartTime, slotEndTime] = timeSlots[i].split(' - ');
                  const cellTimeStart = new Date(reservation.timeStart);
                  cellTimeStart.setHours(parseInt(slotStartTime.split(':')[0]), parseInt(slotStartTime.split(':')[1]), 0, 0);
                  const cellTimeEnd = new Date(reservation.timeStart);
                  cellTimeEnd.setHours(parseInt(slotEndTime.split(':')[0]), parseInt(slotEndTime.split(':')[1]), 0, 0);

                  if (resStartTime < cellTimeEnd && resStartTime >= cellTimeStart) {
                    labReservationCard.top = (resStartTime.getTime() - cellTimeStart.getTime()) * calculatePixelsPerMinute(cellTimeEnd, cellTimeStart);
                    labReservationCard.topCellIndex = i;
                    break;
                  }

                  else if (cellTimeStart >= resStartTime) {
                    labReservationCard.topCellIndex = i;
                    labReservationCard.top = cellFullHeight;
                    labReservationCard.isStartingNextRow = true;
                    break;
                  }
                }

                for (let i = 0; i < timeSlots.length; i++) {
                  const [slotStartTime, slotEndTime] = timeSlots[i].split(' - ');
                  const cellTimeStart = new Date(reservation.timeEnd);
                  cellTimeStart.setHours(parseInt(slotStartTime.split(':')[0]), parseInt(slotStartTime.split(':')[1]), 0, 0);
                  const cellTimeEnd = new Date(reservation.timeEnd);
                  cellTimeEnd.setHours(parseInt(slotEndTime.split(':')[0]), parseInt(slotEndTime.split(':')[1]), 0, 0);

                  if (resEndTime <= cellTimeStart) {
                    if (labReservationCard.top === cellFullHeight) {
                      labReservationCard.height = (i - 1 - labReservationCard.topCellIndex) * cellFullHeight + cellFullHeight;
                    }
                    else {
                      labReservationCard.height = (i - 1 - labReservationCard.topCellIndex) * cellFullHeight + cellFullHeight - labReservationCard.top;
                    }
                    break;
                  }

                  else if (resEndTime <= cellTimeEnd) {
                    labReservationCard.bottomCellIndex = i;
                    const bottom = cellFullHeight - (cellTimeEnd.getTime() - resEndTime.getTime()) * calculatePixelsPerMinute(cellTimeEnd, cellTimeStart);
                    if (labReservationCard.top === cellFullHeight) {
                      labReservationCard.height = (i - 1 - labReservationCard.topCellIndex) * cellFullHeight + bottom + cellFullHeight;
                    }
                    else {
                      labReservationCard.height = (i - 1 - labReservationCard.topCellIndex) * cellFullHeight + bottom + cellFullHeight - labReservationCard.top;
                    }
                    break;
                  }
                }

                labReservationCards.push(labReservationCard);
              }
            }

            const currentDayCellStyle = {
              background: isCurrentDay(cellStartTime) ? "#EFF6FF" : "white"
            };

            return (
              <td key={index} className={style.cell} style={currentDayCellStyle}>
                {labReservationCards.map((labReservationCard, resIndex) => {
                  const overlapStyle = {
                    top: `${labReservationCard.top}px`,
                    height: `${labReservationCard.height}px`,
                  };

                  return (
                    <div
                      key={resIndex}
                      className={style.reservationCard}
                      style={overlapStyle}
                      onClick={() => handleReservationClick(labReservationCard.reservation)}
                    >
                      <div className={style.text}>
                        <span className={style.time}>{`${labReservationCard.reservation.timeStart.split('T')[1].split(':')[0]}:${labReservationCard.reservation.timeStart.split('T')[1].split(':')[1]} -
                        ${labReservationCard.reservation.timeEnd.split('T')[1].split(':')[0]}:${labReservationCard.reservation.timeEnd.split('T')[1].split(':')[1]}`}
                        </span>
                        <span className={style.theme}>{`${labReservationCard.reservation.theme}`}</span>
                      </div>
                    </div>
                  );
                })}
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
