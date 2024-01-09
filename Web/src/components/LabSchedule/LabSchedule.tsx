import { AddLabReservation } from './AddLabReservation/AddLabReservation';
import { ScheduleTable } from './ScheduleTable/ScheduleTable';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { LabReservation, UserRole, Lab } from '../../../api';
import { apiService } from '../../services';
import {userStore} from "../../stores";
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

  const [labs, setLabs] = useState<Lab[]>([]);
  const [selectedLab, setSelectedLab] = useState<Lab | null>(null);

  const fetchLabs = async () => {
    const fetchedLabs = await apiService.getLabs();
    setLabs(fetchedLabs)
  };

  const handleLabSelection = (lab: Lab) => {
    setSelectedLab(lab);
    setIsLabMenuOpen(false);
  };

  const handleAddButtonClick = () => {
    setShowAddModal(true);
  };

  const handleCloseAddModal = () => {
    setShowAddModal(false);
  };

  const handleReservationClick = (reservation: LabReservation) => {
    setSelectedReservation(reservation);
    setShowReservationModal(true);
  };

  const handleCloseReservationModal = () => {
    setShowReservationModal(false);
  };

  const handleDeleteReservationModal = async () => {
    const response = await apiService.deleteLabReservation(selectedReservation?.id, selectedReservation?.reservor?.id)
    if (!(response instanceof Error)) {
      fetchScheduleData(selectedWeek, selectedLab);
    }
    handleCloseReservationModal();
  };

  const fetchScheduleData = async (selectedWeek: Date, selectedLab: Lab | null) => {
    const response = await apiService.getAllLabReservations();

    if (response instanceof Error) {
      return;
    }

    const filteredReservations = response.filter((reservation) => {
      const reservationDate = new Date(reservation.timeStart);
      const reservationWeek = reservationDate.getDate() - reservationDate.getDay();

      const isCorrectWeek = (
        reservationWeek === selectedWeek.getDate() - selectedWeek.getDay() &&
        reservationDate.getFullYear() === selectedWeek.getFullYear() &&
        reservationDate.getMonth() === selectedWeek.getMonth()
      );
  
      const isCorrectLab = selectedLab ? reservation.lab.id === selectedLab.id : false;
  
      return isCorrectWeek && isCorrectLab;
    });
  
    setScheduleData(filteredReservations);
  };

  const goToPreviousWeek = () => {
    const previousWeek = new Date(selectedWeek);
    previousWeek.setDate(selectedWeek.getDate() - 7);
    setSelectedWeek(previousWeek);
    fetchScheduleData(previousWeek, selectedLab);
  };

  const goToNextWeek = () => {
    const nextWeek = new Date(selectedWeek);
    nextWeek.setDate(selectedWeek.getDate() + 7);
    setSelectedWeek(nextWeek);
    fetchScheduleData(nextWeek, selectedLab);
  };

  const toggleLabMenu = () => {
    setIsLabMenuOpen(!isLabMenuOpen);
  };

  useEffect(() => {
    fetchLabs();
  }, []);

  useEffect(() => {
    fetchScheduleData(selectedWeek, selectedLab);
  }, [selectedWeek, selectedLab]);

  return (
    <div id={'lab-schedule'} className={style.container}>
      <div className={style.header}>
        <div className={style.weekNavigation}>
          <LocalButton onClick={goToPreviousWeek}>Назад</LocalButton>
          <div>{selectedWeek.toDateString()}</div>
          <LocalButton onClick={goToNextWeek}>Вперед</LocalButton>
        </div>
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
    {(userStore.user?.role === UserRole.Admin || userStore.user?.role === UserRole.Teacher) &&
        <div className={style.addButton}>
          <LocalButton onClick={handleAddButtonClick}>+ Добавить</LocalButton>
        </div>}
      </div>
      <ScheduleTable
        scheduleData={scheduleData}
        selectedWeek={selectedWeek}
        handleReservationClick={handleReservationClick}
      />
      <div className={style.reservationOverlay} style={{ display: showReservationModal ? 'block' : 'none' }}>
        <div className={style.reservationCardModal}>
          <div className={style.reservationCardContent}>
            <p>{selectedReservation?.timeStart} - {selectedReservation?.timeEnd}</p>
            <p>{selectedReservation?.theme}</p>
            <p>{selectedReservation?.reservor.firstName}</p>
            <p>{selectedReservation?.description}</p>
          </div>
          <LocalButton variant="secondary" onClick={handleCloseReservationModal}>
            Закрыть
          </LocalButton>
          {(userStore.user?.role === UserRole.Admin || (userStore.user?.id === selectedReservation?.id && userStore.user?.role === UserRole.Teacher)) &&
          <LocalButton className={style.deleteButton} variant="secondary" onClick={handleDeleteReservationModal}>
            Удалить
          </LocalButton>}
        </div>
      </div>
        <AddLabReservation
        show={showAddModal}
        handleClose={handleCloseAddModal}
        selectedLab={selectedLab}
        selectedWeek={selectedWeek}
        fetchScheduleData={fetchScheduleData}
      />
    </div>
  );
}