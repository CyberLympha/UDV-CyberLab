import { useState, useEffect, useRef } from 'react';

import { LabReservation, UserRole, Lab } from '../../../api';
import { apiService } from '../../services';
import { userStore } from "../../stores";
import { Button as LocalButton } from '../Button/Button';

import { AddLabReservation } from './AddLabReservation/AddLabReservation';
import { ScheduleTable } from './ScheduleTable/ScheduleTable';
import { LabReservationCard } from "./LabReservationCard/LabReservationCard"
import style from './LabSchedule.module.scss';

export function LabSchedule() {
  const [scheduleData, setScheduleData] = useState<LabReservation[]>([]);
  const [selectedWeek, setSelectedWeek] = useState<Date>(new Date());
  const [isLabMenuOpen, setIsLabMenuOpen] = useState<boolean>(false);
  const [selectedReservation, setSelectedReservation] = useState<LabReservation | null>(null);
  const [showReservationModal, setShowReservationModal] = useState(false);
  const [showAddModal, setShowAddModal] = useState(false);
  const [labs, setLabs] = useState<Lab[]>([]);
  const [selectedLab, setSelectedLab] = useState<Lab | null>(null);
  const [labSelectionWidth, setLabSelectionWidth] = useState<number | null>(null);
  const labSelectionRef = useRef<HTMLDivElement>(null);

  const fetchLabs = async () => {
    const fetchedLabs = await apiService.getLabs();
    if (fetchedLabs instanceof Error){
      return;
    }
    setLabs(fetchedLabs)
    setSelectedLab(fetchedLabs.length > 0 ? fetchedLabs[0] : null);
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

  const handleLabSelection = (lab: Lab) => {
    setSelectedLab(lab);
    setIsLabMenuOpen(false);
  };

  const getWeekRange = (selectedWeek: Date) => {
    const startOfWeek = new Date(selectedWeek);
    startOfWeek.setDate(selectedWeek.getDate() - selectedWeek.getDay());

    const endOfWeek = new Date(startOfWeek);
    endOfWeek.setDate(startOfWeek.getDate() + 6);

    return `${startOfWeek.getDate()} ${startOfWeek.toLocaleDateString(undefined, { month: 'long' })} ${startOfWeek.getFullYear()} -
    ${endOfWeek.getDate()} ${endOfWeek.toLocaleDateString(undefined, { month: 'long' })} ${endOfWeek.getFullYear()}`;
  };

  useEffect(() => {
    fetchLabs();
  }, []);

  useEffect(() => {
    fetchScheduleData(selectedWeek, selectedLab);
  }, [selectedWeek, selectedLab]);

  useEffect(() => {
    if (labSelectionRef.current) {
      setLabSelectionWidth(labSelectionRef.current.offsetWidth);
    }
  }, [labSelectionRef, labs]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        labSelectionRef.current &&
        !labSelectionRef.current.contains(event.target as Node) &&
        isLabMenuOpen
      ) {
        setIsLabMenuOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [isLabMenuOpen]);

  return (
    <div id={'lab-schedule'} className={style.container}>
      <div className={style.header}>
        <div className={style.leftSection}>
          <div className={style.weekNavigation}>
            <LocalButton className={style.weekButton} onClick={goToPreviousWeek}>Назад</LocalButton>
            <div>{getWeekRange(selectedWeek)}</div>
            <LocalButton className={style.weekButton} onClick={goToNextWeek}>Вперед</LocalButton>
          </div>
          <div className={style.labSelection} ref={labSelectionRef}>
            <div className={style.selectedLab} onClick={toggleLabMenu} style={{ width: labSelectionWidth ? `${labSelectionWidth}px` : 'auto' }}>
              {selectedLab ? selectedLab.title : 'Select Lab'} ▼
            </div>
            {isLabMenuOpen && (
              <div className={style.labMenu} style={{ width: labSelectionWidth ? `${labSelectionWidth}px` : 'auto' }}>
                {labs.map((lab) => (
                  <div key={lab.id} onClick={() => handleLabSelection(lab)}>
                    {lab.title}
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
        <div className={style.rightSection}>
          {(userStore.user?.role === UserRole.Admin || userStore.user?.role === UserRole.Teacher) &&
            <div className={style.addButton}>
              <LocalButton onClick={handleAddButtonClick}>+ Добавить</LocalButton>
            </div>}
        </div>
      </div>
      <ScheduleTable
        scheduleData={scheduleData}
        selectedWeek={selectedWeek}
        handleReservationClick={handleReservationClick}
      />
      {selectedReservation != null && 
      <div>
        <LabReservationCard
        selectedReservation={selectedReservation}
        showReservationModal={showReservationModal}
        setShowReservationModal={setShowReservationModal}
        updateTable={() => fetchScheduleData(selectedWeek, selectedLab)}
      />
      </div>}
      <AddLabReservation
        show={showAddModal}
        handleClose={handleCloseAddModal}
        selectedLab={selectedLab}
        selectedWeek={selectedWeek}
        fetchScheduleData={fetchScheduleData}
      />
      <span className={style.note}>*В расписании указано время Екатеринбурга (UTC+5:00)</span>
    </div>
  );
}