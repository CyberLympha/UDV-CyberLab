import Modal from 'react-bootstrap/Modal';

import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import 'react-time-picker/dist/TimePicker.css';

import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {userStore} from "../../stores";
import { LabReservation, UserRole, CreateLabReservationRequest, Lab } from '../../../api';
import { apiService } from '../../services';
import { Button as LocalButton } from '../Button/Button';
import style from './LabSchedule.module.scss';

export function LabSchedule() {
  const [scheduleData, setScheduleData] = useState<LabReservation[]>([]);
  const navigate = useNavigate();
  const [selectedWeek, setSelectedWeek] = useState<Date>(new Date());
  const [isLabMenuOpen, setIsLabMenuOpen] = useState<boolean>(false);
  const [selectedReservation, setSelectedReservation] = useState<LabReservation | null>(null);
  const [showReservationModal, setShowReservationModal] = useState(false);

  const [showAddModal, setShowAddModal] = useState(false);
  const [timeStart, setTimeStart] = useState(new Date());
  const [timeEnd, setTimeEnd] = useState(new Date());
  const [theme, setTheme] = useState('');
  const [description, setDescription] = useState('');

  const [labs, setLabs] = useState<Lab[]>([]);
  const [selectedLab, setSelectedLab] = useState<Lab | null>(null);

  const fetchLabs = async () => {
    const fetchedLabs = await apiService.getLabs();
    setLabs(fetchedLabs)
  };

  const handleLabSelection = (lab: Lab) => {
    setSelectedLab(lab);
    setIsLabMenuOpen(false); // Close lab selection menu
  };

  const handleAddButtonClick = () => {
    setShowAddModal(true);
  };

  const handleCloseAddModal = () => {
    setShowAddModal(false);
  };

  const handleSaveReservation = async () => {
    const newReservation: CreateLabReservationRequest = {
      timeStart: (timeStart.getTime()*10000) + 621355968*1000000000 - timeStart.getTimezoneOffset() * 600000000,
      timeEnd: (timeEnd.getTime() * 10000) + 621355968*1000000000 - timeEnd.getTimezoneOffset() * 600000000,
      theme,
      description,
      reservorId: userStore.user?.id,
      lab: selectedLab,
    };

    const response = await apiService.createLabReservation(newReservation);
    if (!(response instanceof Error)) {
      // Handle success, e.g., close modal, update schedule data, etc.
      setShowAddModal(false);
      // Refresh schedule or update scheduleData state
      fetchScheduleData(selectedWeek, selectedLab);
    } else {
      // Handle error case, e.g., show error message
    }
  };

  const handleReservationClick = (reservation: LabReservation) => {
    setSelectedReservation(reservation);
    setShowReservationModal(true);
  };

  const handleCloseReservationModal = () => {
    setShowReservationModal(false);
  };

  // Function to fetch schedule data for the selected week
  const fetchScheduleData = async (selectedWeek: Date, selectedLab: Lab | null) => {
    const response = await apiService.getAllLabReservations();

    if (response instanceof Error) {
      return;
    }

    // Filter reservations for the selected week
    const filteredReservations = response.filter((reservation) => {
      const reservationDate = new Date(reservation.timeStart);
      const reservationWeek = reservationDate.getDate() - reservationDate.getDay();

      const isCorrectWeek = (
        reservationWeek === selectedWeek.getDate() - selectedWeek.getDay() &&
        reservationDate.getFullYear() === selectedWeek.getFullYear() &&
        reservationDate.getMonth() === selectedWeek.getMonth()
      );
  
      const isCorrectLab = selectedLab ? reservation.lab.id === selectedLab.id : true;
  
      return isCorrectWeek && isCorrectLab;
    });
  
    setScheduleData(filteredReservations);
  };

  // Function to navigate to the previous week
  const goToPreviousWeek = () => {
    const previousWeek = new Date(selectedWeek);
    previousWeek.setDate(selectedWeek.getDate() - 7);
    setSelectedWeek(previousWeek);
    fetchScheduleData(previousWeek, selectedLab);
  };

  // Function to navigate to the next week
  const goToNextWeek = () => {
    const nextWeek = new Date(selectedWeek);
    nextWeek.setDate(selectedWeek.getDate() + 7);
    setSelectedWeek(nextWeek);
    fetchScheduleData(nextWeek, selectedLab);
  };

  // Function to toggle lab menu visibility
  const toggleLabMenu = () => {
    setIsLabMenuOpen(!isLabMenuOpen);
  };

  React.useEffect(() => {
    fetchLabs();
  }, []);

  React.useEffect(() => {
    // Fetch schedule data for the initially selected week
    fetchScheduleData(selectedWeek, selectedLab);
  }, [selectedWeek, selectedLab]);

  // Render schedule table
  const renderScheduleTable = () => {
    const daysOfWeek = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'];
    const timeSlots = ['7:00', '8:00', '9:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00'];
  
    const startDate = new Date(selectedWeek);
    startDate.setDate(startDate.getDate() - startDate.getDay()); // Set to the first day of the selected week
  
    // Generate the table header with dates
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
  
    // Generate the table rows with time slots and reservations
    const tableRows = timeSlots.map((timeSlot, rowIndex) => {
      return (
        <tr key={rowIndex}>
          <td>{timeSlot}</td>
          {Array.from({ length: 7 }, (_, index) => {
            const currentDate = new Date(startDate);
            currentDate.setDate(currentDate.getDate() + index);
  
            // Check for reservations on the current day and time slot
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
  
            // Render reservation details or an empty cell
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
    <div id={'lab-schedule'} className={style.container}>
      {/* Header */}
      <div className={style.header}>
        {/* Week navigation */}
        <div className={style.weekNavigation}>
          <LocalButton onClick={goToPreviousWeek}>Назад</LocalButton>
          <div>{selectedWeek.toDateString()}</div>
          <LocalButton onClick={goToNextWeek}>Вперед</LocalButton>
        </div>
        {/* Lab selection */}
        <div className={style.labSelection}>
      <div className={style.selectedLab} onClick={toggleLabMenu}>
        {selectedLab ? selectedLab.title : 'Select Lab'} ▼
      </div>
      {isLabMenuOpen && (
        <div className={style.labMenu}>
          {labs.map((lab) => (
            <div key={lab.id} onClick={() => handleLabSelection(lab)}>
              {lab.title}
            </div>
          ))}
        </div>
      )}
    </div>
        {/* Add button */}
        <div className={style.addButton}>
          <LocalButton onClick={handleAddButtonClick}>+ Добавить</LocalButton>
        </div>
      </div>
      {/* Weekly Schedule Table */}
      <div className={style.scheduleTable}>
        {renderScheduleTable()}
      </div>
      {/* Reservation card modal */}
      <div className={style.reservationOverlay} style={{ display: showReservationModal ? 'block' : 'none' }}>
        <div className={style.reservationCardModal}>
          <div className={style.reservationCardContent}>
            <p>Full Date: {selectedReservation?.timeStart} - {selectedReservation?.timeEnd}</p>
            <p>Theme: {selectedReservation?.theme}</p>
            <p>Reservor: {selectedReservation?.reservor.firstName}</p>
            <p>Description: {selectedReservation?.description}</p>
          </div>
          <LocalButton variant="secondary" onClick={handleCloseReservationModal}>
            Close
          </LocalButton>
        </div>
      </div>
      <Modal className={style.reservationOverlay} show={showAddModal} onHide={handleCloseAddModal} dialogClassName={style.addReservationModal}>
        <Modal.Header closeButton>
          <Modal.Title>Добавить резервацию</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <form className={style.addReservationForm}>
            <div className={style.formGroup}>
              <label>Тема:</label>
              <input type="text" value={theme} onChange={(e) => setTheme(e.target.value)} />
            </div>
            <div className={style.formGroup}>
              <label>Описание:</label>
              <textarea value={description} onChange={(e) => setDescription(e.target.value)} />
            </div>
            <div className={style.timeInputs}>
              <div className={style.formGroup}>
                <label>Время начала:</label>
                <DatePicker
            selected={timeStart}
            onChange={(date: Date) => setTimeStart(date)}
            showTimeSelect
            timeFormat="HH:mm"
            dateFormat="MMMM d, yyyy h:mm aa"
            className={style.datePicker}
          />
              </div>
              <div className={style.formGroup}>
                <label>Время конца:</label>
                <DatePicker
            selected={timeEnd}
            onChange={(date: Date) => setTimeEnd(date)}
            showTimeSelect
            timeFormat="HH:mm"
            dateFormat="MMMM d, yyyy h:mm aa"
            className={style.datePicker}
          />
              </div>
            </div>
          </form>
        </Modal.Body>
        <Modal.Footer>
        <LocalButton variant="primary" onClick={handleSaveReservation}>
            Сохранить
          </LocalButton>
          <LocalButton variant="secondary" onClick={handleCloseAddModal}>
            Отменить
          </LocalButton>
        </Modal.Footer>
      </Modal>
    </div>
  );
}
